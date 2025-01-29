using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JobManager.Framework.Infrastructure.Scheduler;

public sealed class JobSchedulerService : BackgroundService
{
    private IJobScheduler _scheduler;
    private Timer _timer;

    private readonly IServiceProvider _serviceProvider;

    public JobSchedulerService(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        _scheduler = scope.ServiceProvider.GetRequiredService<IJobScheduler>();

        StartPollingDatabase(stoppingToken);
        return Task.CompletedTask;
    }

    private void StartPollingDatabase(CancellationToken cancellationToken)
    {
        TimeSpan pollingInterval = TimeSpan.FromMinutes(1); // Adjust as needed
        _timer = new Timer(async _ => await _scheduler.ExecuteAsync(_serviceProvider.CreateScope(), cancellationToken),
                                null,
                                TimeSpan.Zero,
                                pollingInterval);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}

