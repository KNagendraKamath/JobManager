using JobManager.Framework.Domain.JobSetup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JobManager.Api.Test.Abstraction;
[System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class BaseTestWebAppFactory: WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile($"appsettings.json");
        });

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            IJobAssemblyProvider jobAssemblyProvider = serviceProvider.GetRequiredService<IJobAssemblyProvider>();
            jobAssemblyProvider.LoadJobsFromAssemblyAsync().GetAwaiter().GetResult();
        });
    }
}
