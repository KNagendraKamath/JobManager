using JobManager.Framework.Domain.JobSchedulerInstance;
using JobManager.Framework.Domain.JobSetup;


namespace JobRunner;

internal sealed class Worker : BackgroundService
{
    private readonly IJobScheduler _scheduler;
    private readonly IJobAssemblyProvider _assemblyProvider;

    public Worker(IJobScheduler jobScheduler,
                  IJobAssemblyProvider jobAssemblyProvider)
    {
        _scheduler = jobScheduler;
        _assemblyProvider = jobAssemblyProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _assemblyProvider.LoadJobsFromAssemblyAsync(stoppingToken);
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

