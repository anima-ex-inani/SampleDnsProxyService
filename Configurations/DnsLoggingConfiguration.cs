namespace SampleDnsProxyService.Configurations;

/// <summary>
/// The configuration used when logging DNS requests.
/// </summary>
/// <param name="Level">
/// The log level to use when logging DNS requests.
/// </param>
/// <param name="LoggedDomains">
/// The domains to log.
/// </param>
#pragma warning disable CA1812
internal sealed record class DnsLoggingConfiguration(
	LogLevel Level,
	[property: ConfigurationKeyName("Domains to Log")]
	IList<string> LoggedDomains
)
{
	public DnsLoggingConfiguration()
		: this(LogLevel.Information, new List<string>())
	{
	}
}
#pragma warning restore CA1812
