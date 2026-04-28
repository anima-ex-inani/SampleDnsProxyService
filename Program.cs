using System.Net;

using Ae.Dns.Client;
using Ae.Dns.Protocol;
using Ae.Dns.Server;

using Microsoft.Extensions.Options;

using SampleDnsProxyService;
using SampleDnsProxyService.Configurations;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions<BlockerConfiguration>()
	.BindConfiguration("DNS Blocking");

builder.Services.AddOptions<PassthroughConfiguration>()
	.BindConfiguration("DNS Passthrough")
	.Validate((configuration) => {
		return configuration.Resolvers.Count > 0 && configuration.Resolvers.All(resolver => IPAddress.TryParse(resolver, out _));
	})
	.ValidateOnStart();

builder.Services.AddOptions<DnsLoggingConfiguration>()
	.BindConfiguration("DNS Request Logging");

builder.Services.AddOptions<DnsUdpServerOptions>()
	.Configure((options) => {
		options.Endpoint = new IPEndPoint(IPAddress.Any, 53);
	});

builder.Services.AddSingleton<IDnsClient, CustomBlockingDnsClient>();
builder.Services.AddSingleton<IDnsRawClient, DnsRawClient>();
builder.Services.AddSingleton<IDnsServer>((services) => {
	var options = services.GetRequiredService<IOptions<DnsUdpServerOptions>>();
	var client = services.GetRequiredService<IDnsRawClient>();
	return new DnsUdpServer(client, options.Value);
});

builder.Services.AddHostedService<DnsProxyService>();

var host = builder.Build();
await host.RunAsync().ConfigureAwait(ConfigureAwaitOptions.None);
