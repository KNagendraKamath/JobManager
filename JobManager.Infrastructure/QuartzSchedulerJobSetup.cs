using JobManager.Infrastructure.JobSchedulerInstance.Scheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace JobManager.Infrastructure;

public class QuartzSchedulerJobSetup : IConfigureOptions<QuartzOptions>
{
    private readonly ILogger<QuartzSchedulerJobSetup> _logger;
    private readonly ISchedulerFactory _schedulerFactory;
    public QuartzSchedulerJobSetup(ILogger<QuartzSchedulerJobSetup> logger,ISchedulerFactory schedulerFactory)
    {
        _logger = logger;
        _schedulerFactory = schedulerFactory;
    }

    public async void Configure(QuartzOptions options)
    {
        _logger.LogInformation("Configuring Quartz Scheduler");

        const string jobName = "QuartzScheduler";

        options
            .AddJob<QuartzScheduler>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                     configure
                    .ForJob(jobName)
                    .StartNow()
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(60).RepeatForever()));
    }
}
