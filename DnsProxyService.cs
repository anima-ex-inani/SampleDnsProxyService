using Ae.Dns.Protocol;

namespace SampleDnsProxyService;

#pragma warning disable CA1812
internal sealed partial class DnsProxyService(IDnsServer server)
	: BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await server.Listen(stoppingToken)
			.ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext | ConfigureAwaitOptions.SuppressThrowing);
	}
}
#pragma warning restore CA1812
