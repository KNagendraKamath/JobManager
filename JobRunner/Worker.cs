using JobManager.Application.JobSetup.GetJobDetail;
using JobManager.Domain.Abstractions;
using MediatR;
using Quartz;

namespace JobRunner;

public class Worker : BackgroundService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private IScheduler _scheduler;
    private readonly ISender _sender;
    private readonly ILogger<Worker> _logger;


    public Worker(ISchedulerFactory schedulerFactory, ISender sender, ILogger<Worker> logger)
    {
        _schedulerFactory = schedulerFactory;
        _sender = sender;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _scheduler = await _schedulerFactory.GetScheduler(stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            StartPollingDatabase(stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task ScheduleJobsFromDatabase(CancellationToken cancellationToken)
    {
        try
        {
            Result<List<JobResponse>> jobsToBeScheduled = await _sender.Send(new GetPendingOneTimeAndRecurringJobQuery(), cancellationToken);

            foreach (JobResponse jobResponse in jobsToBeScheduled.Value)
            {
                string jobGroup = jobResponse.JobId.ToString();

                foreach (JobStepResponse jobStep in jobResponse.Steps)
                {
                    Type jobClass = Type.GetType(jobStep.JobConfigName)!;

                    long jobIdentifierId = jobStep.JobStepId;

                    IJobDetail job = JobBuilder.Create(jobClass)
                                        .WithIdentity(jobIdentifierId.ToString(), jobGroup)
                                        .Build();

                    ITrigger trigger = TriggerBuilder.Create()
                                                .WithIdentity(jobIdentifierId.ToString(), jobGroup)
                                                .StartNow()
                                                .Build();

                    await _scheduler.ScheduleJob(job, trigger, cancellationToken);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

    }

    private void StartPollingDatabase(CancellationToken cancellationToken)
    {
        TimeSpan pollingInterval = TimeSpan.FromMinutes(1); // Adjust as needed
        Timer timer = new Timer(async _ => await ScheduleJobsFromDatabase(cancellationToken),
                                null,
                                pollingInterval,
                                pollingInterval);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _scheduler.Shutdown(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}

