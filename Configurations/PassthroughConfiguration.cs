namespace SampleDnsProxyService.Configurations;

/// <summary>
/// The configuration used for the DNS passthrough.
/// </summary>
/// <param name="Resolvers">
/// The resolvers to use for the DNS passthrough. If multiple resolvers are specified, they will be used in the order
/// they are specified.
/// </param>
internal record class PassthroughConfiguration(
	IList<string> Resolvers
);
