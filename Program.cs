using SampleDnsProxyService;
using SampleDnsProxyService.Configurations;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions<BlockerConfiguration>()
	.BindConfiguration("Blocker Configuration");

builder.Services.AddHostedService<DnsProxyService>();

var host = builder.Build();
await host.RunAsync().ConfigureAwait(ConfigureAwaitOptions.None);
