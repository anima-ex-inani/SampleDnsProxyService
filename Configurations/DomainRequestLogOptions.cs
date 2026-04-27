namespace SampleDnsProxyService.Configurations;

/// <summary>
/// The options used when logging domain requests.
/// </summary>
/// <param name="Level">The log level to use when logging domain name requests.</param>
/// <param name="IncludeResponse">Whether the response from forwarded requests should be included in the logs.</param>
/// <param name="LogBlockedDomains">
/// If <see langword="true"/>, blocked domains will be logged even if the domain was not specified to be logged.
/// </param>
internal record class DomainRequestLogOptions(
	LogLevel Level,
	[property: ConfigurationKeyName("Include Response")]
	bool IncludeResponse,
	[property: ConfigurationKeyName("Log Blocked Domains")]
	bool LogBlockedDomains
);
