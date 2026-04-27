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
	ISet<string> LoggedDomains
)
{
	public DnsLoggingConfiguration()
		: this(LogLevel.Information, new HashSet<string>(StringComparer.OrdinalIgnoreCase))
	{
	}
}
#pragma warning restore CA1812
