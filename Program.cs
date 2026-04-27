using SampleDnsProxyService;
using SampleDnsProxyService.Configurations;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions<BlockerConfiguration>()
	.BindConfiguration("DNS Blocking");
builder.Services.AddOptions<PassthroughConfiguration>()
	.BindConfiguration("DNS Passthrough");
builder.Services.AddOptions<DnsLoggingConfiguration>()
	.BindConfiguration("DNS Request Logging");

builder.Services.AddHostedService<DnsProxyService>();

var host = builder.Build();
await host.RunAsync().ConfigureAwait(ConfigureAwaitOptions.None);
