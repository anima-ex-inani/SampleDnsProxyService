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
internal record class DnsLoggingConfiguration(
	LogLevel Level,
	[property: ConfigurationKeyName("Domains to Log")]
	IList<string> LoggedDomains
);
