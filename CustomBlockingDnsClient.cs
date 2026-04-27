using System.Diagnostics;
using System.Net;

using Ae.Dns.Client;
using Ae.Dns.Protocol;
using Ae.Dns.Protocol.Enums;
using Ae.Dns.Protocol.Records;

using Microsoft.Extensions.Options;

using SampleDnsProxyService.Configurations;

namespace SampleDnsProxyService;

#pragma warning disable CA1812
internal sealed partial class CustomBlockingDnsClient
	: IDnsClient
{
	private readonly ILogger<CustomBlockingDnsClient> _logger;
	private readonly IOptionsMonitor<BlockerConfiguration> _blockerOptions;
	private readonly IOptionsMonitor<DnsLoggingConfiguration> _loggingOptions;
	private bool _disposed;
	private readonly DnsRandomClient _passthroughClient;

	public CustomBlockingDnsClient(
		ILogger<CustomBlockingDnsClient> logger,
		IOptionsMonitor<BlockerConfiguration> blockerOptions,
		IOptions<PassthroughConfiguration> passthroughOptions,
		IOptionsMonitor<DnsLoggingConfiguration> loggingOptions
	)
	{
		_logger = logger;
		_blockerOptions = blockerOptions;
		_loggingOptions = loggingOptions;

		var currentPassthroughConfiguration = passthroughOptions.Value;
		_passthroughClient = new DnsRandomClient(
			currentPassthroughConfiguration.Resolvers
				.Select(resolver => new DnsUdpClient(IPAddress.Parse(resolver)))
		);
	}

	private static Func<ILogger, int, string, IDisposable?> s_queryLogScope =
		LoggerMessage.DefineScope<int, string>(
			"Query {Id}: {Domain}"
		);

	[LoggerMessage(LogLevel.Information, "{Timestamp}: Received query {RecordType} records of {Domain}")]
	private static partial void LogQuery(ILogger logger, DateTime timestamp, string domain, DnsQueryType RecordType);

	[LoggerMessage(LogLevel.Information, "Blocked query for {Domain}; Returned {ResponseType})")]
	private static partial void LogBlockedQuery(ILogger logger, string domain, BlockerResponseType responseType);

	private static DnsMessage CreateBlockedResponse(DnsMessage query, BlockerResponseType strategy)
	{
		return new DnsMessage {
			Header = {
				Id = query.Header.Id,
				QuestionCount = 1,
				AnswerRecordCount = strategy switch {
					BlockerResponseType.NonExistentDomain => 0,
					BlockerResponseType.NoData => 0,
					BlockerResponseType.Refused => 0,
					BlockerResponseType.Sinkhole => 1,
					_ => throw new UnreachableException(),
				},
				NameServerRecordCount = 0,
				AdditionalRecordCount = 0,
				QueryType = query.Header.QueryType,
				QueryClass = query.Header.QueryClass,
				IsQueryResponse = true,
				OperationCode = query.Header.OperationCode,
				AuthoritativeAnswer = true,
				RecursionDesired = query.Header.RecursionDesired,
				RecursionAvailable = true,
				ResponseCode = strategy switch {
					BlockerResponseType.NonExistentDomain => DnsResponseCode.NXDomain,
					BlockerResponseType.NoData => DnsResponseCode.NoError,
					BlockerResponseType.Refused => DnsResponseCode.Refused,
					BlockerResponseType.Sinkhole => DnsResponseCode.NoError,
					_ => throw new UnreachableException(),
				},
				Host = query.Header.Host,
			},
			Answers = strategy switch {
				BlockerResponseType.NonExistentDomain => Array.Empty<DnsResourceRecord>(),
				BlockerResponseType.NoData => Array.Empty<DnsResourceRecord>(),
				BlockerResponseType.Refused => Array.Empty<DnsResourceRecord>(),
				BlockerResponseType.Sinkhole => [
					new DnsResourceRecord {
						Class = DnsQueryClass.IN,
						Host = query.Header.Host,
						Resource = new DnsIpAddressResource {
							IPAddress = query.Header.QueryType switch {
								DnsQueryType.A => IPAddress.Any,
								DnsQueryType.AAAA => IPAddress.IPv6Any,
								_ => throw new ArgumentOutOfRangeException(nameof(query)),
							},
						},
						Type = query.Header.QueryType,
					},
				],
				_ => throw new UnreachableException(),
			},
			Additional = Array.Empty<DnsResourceRecord>(),
			Nameservers = Array.Empty<DnsResourceRecord>(),
		};
	}

	public async Task<DnsMessage> Query(DnsMessage query, CancellationToken token = default(CancellationToken))
	{
		var blockerOptions = _blockerOptions.CurrentValue;

		// We only want to block queries for A and AAAA records. Anything else is sent to the passthrough
		// client.
		if (query.Header.QueryType is not (DnsQueryType.A or DnsQueryType.AAAA)) {
			return await _passthroughClient.Query(query, token).ConfigureAwait(false);
		}

		var fullHost = string.Join(".", (IReadOnlyList<string>)query.Header.Host);
		using var logScope = s_queryLogScope(_logger, query.Header.Id, fullHost);

		if (_loggingOptions.CurrentValue.LoggedDomains.Contains(fullHost, StringComparer.InvariantCultureIgnoreCase)) {
			LogQuery(_logger, DateTime.Now, fullHost, query.Header.QueryType);
		}

		for (int i = 0; i < query.Header.Host.Count; i++) {
			// This method makes the blocker check not just if the domain is being blocked, but also if any of its parent domains are being blocked.
			// For example, if "www.google.com" is being queried, and "google.com" is in the blocked domains list, this method will block the query
			// for "www.google.com" as well.
			var domain = string.Join(".", query.Header.Host.Skip(i));

			if (!blockerOptions.BlockedDomains.TryGetValue(domain, out var strategy)) {
				continue;
			}

			if (blockerOptions.LogBlockedDomains) {
				LogBlockedQuery(_logger, domain, strategy);
			}

			return CreateBlockedResponse(query, strategy);
		}

		return await _passthroughClient.Query(query, token).ConfigureAwait(false);
	}

	public void Dispose()
	{
		if (Interlocked.Exchange(ref _disposed, true)) {
			return;
		}

		_passthroughClient.Dispose();
	}
}
#pragma warning restore CA1812
