namespace SampleDnsProxyService;

#pragma warning disable CA1812
internal sealed partial class DnsProxyService(ILogger<DnsProxyService> logger)
	: BackgroundService
{
	[LoggerMessage(LogLevel.Information, "Worker running at: {Time}")]
	private static partial void LogCurrentTime(ILogger logger, DateTimeOffset time);

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested) {
			LogCurrentTime(logger, DateTimeOffset.Now);
			await Task.Delay(1000, stoppingToken).ConfigureAwait(false);
		}
	}
}
#pragma warning restore CA1812
