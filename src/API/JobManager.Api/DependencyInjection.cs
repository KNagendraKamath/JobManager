using JobManager.Framework.Application;
using JobManager.Framework.Infrastructure;

namespace JobManager.Api;

internal static class DependencyInjection
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);
        return services;
    }
}
