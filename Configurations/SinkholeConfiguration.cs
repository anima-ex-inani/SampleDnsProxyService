namespace SampleDnsProxyService.Configurations;

/// <summary>
/// The configuration used when a domain is sinkholed.
/// </summary>
/// <param name="TargetAddress">
/// The IP address to resolve sinkholed domains to.
/// </param>
internal record class SinkholeConfiguration(
	[property: ConfigurationKeyName("Target Address")]
	string TargetAddress
);
