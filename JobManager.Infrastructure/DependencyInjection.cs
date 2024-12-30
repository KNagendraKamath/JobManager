using System.Diagnostics;
using JobManager.Application.Abstractions.Database;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;
using JobManager.Domain.JobSetup;
using JobManager.Infrastructure.Abstractions;
using JobManager.Infrastructure.JobSchedulerInstance;
using JobManager.Infrastructure.JobSchedulerInstance.Scheduler;
using JobManager.Infrastructure.JobSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz.Impl;
using Quartz;
using Quartz.Spi;

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
        services.AddQuartz();
        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });
        services.AddSingleton<IJobFactory, JobFactory>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
    }

    private static IServiceCollection AddRepository(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Database") ??
                                       throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<JobDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
                    .UseLowerCaseNamingConvention()
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
