using System.Diagnostics;
using JobManager.Application.Abstractions.Database;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;
using JobManager.Domain.JobSetup;
using JobManager.Infrastructure.Abstractions;
using JobManager.Infrastructure.JobSchedulerInstance;
using JobManager.Infrastructure.JobSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz.Impl;
using Quartz;
using JobManager.Infrastructure.JobSchedulerInstance.Scheduler;

namespace JobManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        
        AddRepository(services, configuration);
        AddScheduler(services);

     
        return services;
    }


    private static void AddScheduler(IServiceCollection services)
    {
      
        services.AddQuartz(q=> q.AddJob<QuartzScheduler>(configure => configure.WithIdentity("QuartzScheduler"))
            .AddTrigger(configure =>
                     configure
                    .ForJob("QuartzScheduler")
                    .StartNow()
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(60).RepeatForever())));

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true); 

        services.AddSingleton<IJobScheduler,QuartzScheduler>();

    }

    private static IServiceCollection AddRepository(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Database") ??
                                       throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<JobDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
                   .UseSnakeCaseNamingConvention()
                   .LogTo(message => Debug.WriteLine(message), LogLevel.Trace);
        });

        services.AddScoped<IJobConfigRepository, JobConfigRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IJobStepRepository, JobStepRepository>();
        services.AddScoped<IJobInstanceRepository, JobInstanceRepository>();
        services.AddScoped<IJobStepInstanceRepository, JobStepInstanceRepository>();
        services.AddScoped<IJobStepInstanceLogRepository, JobStepInstanceLogRepository>();

        services.AddScoped<IUnitOfWork>(s => s.GetRequiredService<JobDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(connectionString));

        return services;
    }
}
