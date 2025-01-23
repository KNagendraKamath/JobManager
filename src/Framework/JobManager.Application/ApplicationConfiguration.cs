using FluentValidation;
using JobManager.Framework.Application.Abstractions.Behaviors;
using JobManager.Framework.Application.JobSchedulerInstance;
using JobManager.Framework.Application.JobSetup;
using JobManager.Framework.Application.JobSetup.ScheduleJob;
using JobManager.Framework.Domain.JobSchedulerInstance;
using JobManager.Framework.Domain.JobSetup;
using Microsoft.Extensions.DependencyInjection;

namespace JobManager.Framework.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(ApplicationConfiguration).Assembly);
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationConfiguration).Assembly, includeInternalTypes: true);

        AddValidationBehavior(services);
        services.AddScoped<ICronExpressionGenerator, QuartzCronExpressionGenerator>();

        return services;
    }

    private static void AddValidationBehavior(IServiceCollection services)
    {
        services.AddScoped<IJobConfigValidation,JobConfigValidation>();
        services.AddScoped<IJobValidation,JobValidation>();
        services.AddScoped<IJobStepValidation,JobStepValidation>();
        services.AddScoped<IJobInstanceValidation,JobInstanceValidation>();
        services.AddScoped<IJobStepInstanceValidation,JobStepInstanceValidation>();
    }
}
