using DDV.Jobs;
using Job.ProjectLayer;
using JobManager.Framework.Domain.JobSetup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Job.Modules.Common;

public static class Class1
{
    public static IServiceCollection AddJobRoutines(
     this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDDVJobs();
        services.AddTestJob();
        services.AddSingleton<IJobAssemblyProvider, JobAssemblyProvider>();
        return services;
    }

}
