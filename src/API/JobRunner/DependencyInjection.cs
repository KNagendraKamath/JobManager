using Job.Modules.Common;
using JobManager.Framework.Application;
using JobManager.Framework.Infrastructure;

namespace JobRunner;

internal static class DependencyInjection
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddApplication();
        services.AddJobRoutines(configuration);

        return services;
    }
}
