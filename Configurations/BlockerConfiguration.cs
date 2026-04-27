namespace SampleDnsProxyService.Configurations;

/// <summary>
/// The configuration used for the DNS blocker.
/// </summary>
/// <param name="Sinkhole">
/// The configuration used when a domain is sinkholed.
/// </param>
/// <param name="PassthroughAddress">
/// The IP address to forward non-blocked requests to.
/// </param>
/// <param name="BlockedDomains">
/// The domains that are blocked along with the response type for each domain.
/// </param>
/// <param name="LoggedDomains">
/// The domains which are logged when requested. These domains are not blocked automatically; they must also be listed
/// in <paramref name="BlockedDomains"/> to be blocked.
/// </param>
/// <param name="LogOptions">
/// The options used when logging domain requests.
/// </param>
internal record class BlockerConfiguration(
	[property: ConfigurationKeyName("Sinkhole Configuration")]
	SinkholeConfiguration Sinkhole,
	[property: ConfigurationKeyName("Passthrough Address")]
	string PassthroughAddress,
	[property: ConfigurationKeyName("Blocked Domains")]
	IDictionary<string, BlockerResponseType> BlockedDomains,
	[property: ConfigurationKeyName("Logged Domains")]
	IReadOnlyList<string> LoggedDomains,
	[property: ConfigurationKeyName("Log Options")]
	DomainRequestLogOptions LogOptions
);
