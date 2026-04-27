using SampleDnsProxyService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<DnsProxyService>();

var host = builder.Build();
host.Run();
