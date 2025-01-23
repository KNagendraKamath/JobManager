using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobRunner;

internal sealed class Worker : BackgroundService
{
    private readonly IJobScheduler _scheduler;

    public Worker(IJobScheduler scheduler) => 
        _scheduler = scheduler;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        StartPollingDatabase(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
            await Task.Delay(1000, stoppingToken);
    }


    private void StartPollingDatabase(CancellationToken cancellationToken)
    {
        TimeSpan pollingInterval = TimeSpan.FromMinutes(1); // Adjust as needed
        using Timer timer = new(async _ => await _scheduler.ExecuteAsync(cancellationToken),
                                null,
                                TimeSpan.Zero,
                                pollingInterval);
    }

    public override async Task StopAsync(CancellationToken cancellationToken) =>
        await base.StopAsync(cancellationToken);
}

