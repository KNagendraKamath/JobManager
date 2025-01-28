using JobRunner;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddLogging();
builder.Logging.AddConsole();

builder.Services.AddHostedService<Worker>();

builder.Services.AddDependencies(builder.Configuration);

IHost host = builder.Build();


await host.RunAsync();


