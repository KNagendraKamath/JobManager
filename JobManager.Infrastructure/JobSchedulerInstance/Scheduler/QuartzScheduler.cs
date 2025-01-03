using JobManager.Application.JobSetup.GetJobDetail;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSetup;
using MediatR;
using Microsoft.Extensions.Hosting;
using Quartz.Impl.Matchers;
using Quartz;
using JobManager.Infrastructure.Abstractions;

namespace JobManager.Infrastructure.JobSchedulerInstance.Scheduler;
public class QuartzScheduler : BackgroundService
{
    private ISchedulerFactory SchedulerFactory { get; set; } = ServiceLocator.GetInstance<ISchedulerFactory>();
    private IScheduler Scheduler { get; set; } 
    private ISender Sender { get; set; } = ServiceLocator.GetInstance<ISender>();
    private Timer _timer;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Scheduler = await SchedulerFactory.GetScheduler(stoppingToken);

        StartPollingDatabase(stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
            await Task.Delay(1000, stoppingToken);

    }

    private async Task ScheduleJobsFromDatabase(CancellationToken cancellationToken)
    {
        try
        {
            IReadOnlyCollection<JobKey> jobKeys = await Scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup(), cancellationToken);
            string scheduledJobIds = string.Join(",", jobKeys.Select(j => j.Group));

            Result<List<JobResponse>> jobsToBeScheduled = await Sender.Send(new GetPendingOneTimeAndRecurringJobQuery(scheduledJobIds), cancellationToken);

            Dictionary<long, CronScheduleBuilder> JobCrons = (from job in (jobsToBeScheduled.Value ?? new())
                                                              group job.RecurringDetail by job.JobId into jobGroup
                                                              let cronExpression = CronExpressionGenerator(jobGroup.FirstOrDefault())
                                                              select new
                                                              {
                                                                  JobId = jobGroup.Key,
                                                                  CronExpression = cronExpression
                                                              }).ToDictionary(j => j.JobId, j => j.CronExpression);

            foreach (JobResponse jobResponse in jobsToBeScheduled.Value ?? new())
            {
                Type jobClass = Type.GetType(jobResponse.JobConfigName)!;

                IJobDetail job = JobBuilder.Create(jobClass)
                                    .WithIdentity($"{jobResponse.JobStepId}", $"{jobResponse.JobId}")
                                    .UsingJobData("jsonParameter", jobResponse.JsonParameter)
                                    .Build();

                TriggerBuilder triggerBuilder = TriggerBuilder.Create()
                                       .WithIdentity($"{jobResponse.JobStepId}", $"{jobResponse.JobId}")
                                       .StartAt(jobResponse.EffectiveDateTime);

                CronScheduleBuilder cronExpression = JobCrons[jobResponse.JobId];
                if (cronExpression is not null)
                    triggerBuilder.WithSchedule(cronExpression);
                

                await Scheduler.ScheduleJob(job, triggerBuilder.Build(), cancellationToken);
            }
        }
        catch (Exception ex)
        {
        }
    }

    private CronScheduleBuilder CronExpressionGenerator(RecurringDetailResponse? recurringDetail)
    {
        if (recurringDetail is null)
            return null;

        return recurringDetail.RecurringType switch
        {
            RecurringType.EveryNoSecond => CronScheduleBuilder.CronSchedule($"0/{recurringDetail.Second ?? 0} * * * * ? *"),
            RecurringType.EveryNoMinute => CronScheduleBuilder.CronSchedule($"0 0/{recurringDetail.Minutes ?? 0} * 1/1 * ? *"),
            RecurringType.Daily => CronScheduleBuilder.DailyAtHourAndMinute(recurringDetail.Hours ?? 0, recurringDetail.Minutes ?? 0),
            RecurringType.Weekly => CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(
                                        (DayOfWeek)Enum.Parse(typeof(DayOfWeek),
                                                    recurringDetail.DayOfWeek.ToString()!,
                                                    true),
                                        recurringDetail.Hours ?? 0,
                                        recurringDetail.Minutes ?? 0),
            RecurringType.Monthly => CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(
                recurringDetail.Day ?? 0, recurringDetail.Hours ?? 0, recurringDetail.Minutes ?? 0),
            _ => throw new NotImplementedException($"Recurring type {recurringDetail.RecurringType} is not supported")
        };
    }

    private void StartPollingDatabase(CancellationToken cancellationToken)
    {
        TimeSpan pollingInterval = TimeSpan.FromMinutes(1); // Adjust as needed
        _timer = new Timer(async _ => await ScheduleJobsFromDatabase(cancellationToken),
                           null,
                           TimeSpan.Zero,
                           pollingInterval);

    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0); // Stop the timer
        await Scheduler.Shutdown(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
