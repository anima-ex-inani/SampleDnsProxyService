namespace SampleDnsProxyService.Configurations;

internal enum BlockerResponseType
{
	NonExistentDomain,
	NoData,
	Refused,
	Sinkhole,
}
