using Microsoft.Extensions.DependencyInjection;

namespace Job.ProjectLayer;
public static class DependencyInjection
{
    public static IServiceCollection AddProjectJob(this IServiceCollection services)
    {
        services.AddTransient<Test>();
        return services;
    }
}
