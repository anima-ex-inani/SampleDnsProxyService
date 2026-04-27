namespace SampleDnsProxyService.Configurations;

/// <summary>
/// The various response types that can be returned for blocked queries.
/// </summary>
internal enum BlockerResponseType
{
	/// <summary>
	/// The blocker returns an <c>NXDOMAIN</c> response, indicating that the domain does not exist.
	/// </summary>
	NonExistentDomain,

	/// <summary>
	/// The blocker returns a <c>NOERROR</c> response with no answer records.
	/// </summary>
	NoData,

	/// <summary>
	/// The blocker returns a <c>REFUSED</c> response, indicating that the server refuses to perform the operation for policy reasons.
	/// </summary>
	Refused,

	/// <summary>
	/// The blocker returns a <c>NOERROR</c> response with a single record pointing to an IP address guaranteed to refuse any attempts
	/// to connect to it (e.g., <c>0.0.0.0</c>).
	/// </summary>
	Sinkhole,
}
