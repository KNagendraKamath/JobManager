using JobManager.Framework.Domain.JobSetup;
using JobManager.Framework.Infrastructure.JobSchedulerInstance.Scheduler;


namespace JobRunner;

internal sealed class Worker : BackgroundService
{
    private IJobScheduler _scheduler;
    private Timer _timer;

    private readonly IServiceProvider _serviceProvider;

    public Worker(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        _scheduler = scope.ServiceProvider.GetRequiredService<IJobScheduler>();

        IJobAssemblyProvider assemblyProvider = scope.ServiceProvider.GetRequiredService<IJobAssemblyProvider>();
        await assemblyProvider.LoadJobsFromAssemblyAsync(stoppingToken);

        StartPollingDatabase(stoppingToken);
    }

    private void StartPollingDatabase(CancellationToken cancellationToken)
    {
        TimeSpan pollingInterval = TimeSpan.FromMinutes(1); // Adjust as needed
        _timer = new Timer(async _ => await _scheduler.ExecuteAsync(_serviceProvider.CreateScope(),cancellationToken),
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

