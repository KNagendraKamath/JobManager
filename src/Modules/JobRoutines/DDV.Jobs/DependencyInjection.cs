using Microsoft.Extensions.DependencyInjection;
namespace DDV.Jobs;
public static class DependencyInjection
{
    public static IServiceCollection AddDDVJobs(this IServiceCollection services)
    {
        services.AddScoped<IVaccineService,VaccineService>();
        services.AddScoped<MetadataServiceUpdateTask>();
        return services;
    }
}
