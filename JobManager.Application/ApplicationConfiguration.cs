using FluentValidation;
using JobManager.Framework.Application.Abstractions.Behaviors;
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

        return services;
    }

    private static void AddValidationBehavior(IServiceCollection services)
    {
       
    }
}
