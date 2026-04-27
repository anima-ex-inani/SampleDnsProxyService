namespace SampleDnsProxyService.Configurations;

/// <summary>
/// The configuration used for the DNS passthrough.
/// </summary>
/// <param name="Resolvers">
/// The resolvers to use for the DNS passthrough. If multiple resolvers are specified, a random one will be chosen for
/// each query.
/// </param>
#pragma warning disable CA1812
internal sealed record class PassthroughConfiguration(
	IList<string> Resolvers
)
{
	public PassthroughConfiguration()
		: this(new List<string>())
	{
	}
}
#pragma warning restore CA1812
