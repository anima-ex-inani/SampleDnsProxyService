namespace SampleDnsProxyService.Configurations;

/// <summary>
/// The configuration used for the DNS blocker.
/// </summary>
/// <param name="SinkholeTarget">
/// The IP address to return when a domain is sinkholed.
/// </param>
/// <param name="BlockedDomains">
/// The domains that are blocked along with the response type for each domain.
/// </param>
/// <param name="LogBlockedDomains">
/// Whether blocked domains should be logged regardless of whether they are specified to be logged.
/// </param>
internal record class BlockerConfiguration(
	[property: ConfigurationKeyName("Sinkhole Target")]
	string SinkholeTarget,
	[property: ConfigurationKeyName("Blocked Domains")]
	IDictionary<string, BlockerResponseType> BlockedDomains,
	[property: ConfigurationKeyName("Log Blocked Domains")]
	bool LogBlockedDomains
);
