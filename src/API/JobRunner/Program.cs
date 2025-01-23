using JobRunner;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDependencies(builder.Configuration);

IHost host = builder.Build();
host.Run();
