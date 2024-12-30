using Job.ProjectLayer;
using JobManager.Application;
using JobManager.Infrastructure;
using JobManager.Infrastructure.Abstractions;
using JobRunner;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();


builder.Services.AddProjectJob();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

ServiceLocator.Current = builder.Services.BuildServiceProvider();
IHost host = builder.Build();
host.Run();
