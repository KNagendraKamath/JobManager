using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Job.Modules.Common;

public static class Class1
{
    public static IServiceCollection AddJobRoutines(
     this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }

}
