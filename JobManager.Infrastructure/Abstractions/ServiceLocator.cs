using Microsoft.Extensions.DependencyInjection;

namespace JobManager.Infrastructure.Abstractions;
public static class ServiceLocator
{
    private static IServiceProvider _currentServiceProvider;

    public static IServiceProvider Current
    {
        get => _currentServiceProvider ?? throw new InvalidOperationException("Service provider not set.");
        set => _currentServiceProvider = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static T GetInstance<T>() where T : notnull => Current.GetRequiredService<T>();
}
