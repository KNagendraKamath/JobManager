using Dapper;
using JobManager.Framework.Application.Abstractions.Database;
using JobManager.Framework.Application.JobSetup;
using JobManager.Framework.Domain.JobSchedulerInstance;
using JobManager.Framework.Domain.JobSetup;
using JobManager.Framework.Infrastructure.Abstractions;
using JobManager.Framework.Infrastructure.JobSchedulerInstance;
using JobManager.Framework.Infrastructure.JobSchedulerInstance.Scheduler;
using JobManager.Framework.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;
using JobManager.Framework.Infrastructure.JobSetup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl.Matchers;

namespace JobManager.Framework.Infrastructure;

public static class JobManagerModule
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
        services.AddQuartz(q=> q.AddJobListener<JobListener>(GroupMatcher<JobKey>.AnyGroup()));
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true); 
        services.AddScoped<IJobScheduler,Scheduler>();
    }

    private static void AddRepository(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Database") ??
                                       throw new ArgumentNullException(nameof(configuration));

        services.AddScoped<ISqlConnectionFactory>(_ =>
    new SqlConnectionFactory(connectionString));

        //Enum type handlers for Dapper SQL
        SqlMapper.AddTypeHandler(new EnumTypeHandler<Status>());
        SqlMapper.AddTypeHandler(new EnumTypeHandler<JobType>());
        SqlMapper.AddTypeHandler(new EnumTypeHandler<RecurringType>());

        // Add Repositories
        services.AddScoped<IJobConfigRepository, JobConfigRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IJobStepRepository, JobStepRepository>();
        services.AddScoped<IJobInstanceRepository, JobInstanceRepository>();
        services.AddScoped<IJobStepInstanceRepository, JobStepInstanceRepository>();
        services.AddScoped<IJobStepInstanceLogRepository, JobStepInstanceLogRepository>();

        //Add Query
        services.AddScoped<IJobQuery, JobQuery>();



    }
}
