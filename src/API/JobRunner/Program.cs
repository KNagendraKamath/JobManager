using JobRunner;
using JobManager.Framework.Infrastructure.Scheduler;
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddLogging();
builder.Logging.AddConsole();

builder.Services.AddHostedService<JobSchedulerService>();

builder.Services.AddDependencies(builder.Configuration);

IHost host = builder.Build();


await host.RunAsync();


