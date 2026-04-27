using SampleDnsProxyService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<DnsFilteringService>();

var host = builder.Build();
host.Run();
