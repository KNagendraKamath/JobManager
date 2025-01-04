using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;


namespace JobmanagerTest.Abstract;

public class BaseTestWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile($"appsettings.Development.json");
        });
        builder.ConfigureTestServices(services =>
        {
            IocConfig.Configure(services);
            services.AddScoped<IApiKeyStorage, TestApiKeyStorage>();
        });
    }
}
