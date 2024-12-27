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

namespace JobManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
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
        services.AddScoped<IJobRepository,JobRepository>();
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
