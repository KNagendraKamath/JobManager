using JobManager.Application.JobSetup.GetJobDetail;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSetup;
using MediatR;
using Quartz.Impl.Matchers;
using Quartz;
using Microsoft.Extensions.Logging;
using JobManager.Domain.JobSchedulerInstance;
using Quartz.Listener;

namespace JobManager.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;

internal class Scheduler : IJobScheduler
{
    private readonly ISchedulerFactory _schedulerFactory;
    private IScheduler _scheduler;
    private readonly ISender _sender;
    private readonly ILogger<Scheduler> _logger;

    public Scheduler(ISchedulerFactory schedulerFactory,
                           ISender sender,
                           ILogger<Scheduler> logger)
    {
        _schedulerFactory = schedulerFactory;
        _sender = sender;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
      
        IReadOnlyCollection<JobKey> jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup(), cancellationToken);
        string alreadyScheduledJobIds = string.Join(",", jobKeys.Select(j => j.Group));

        Result<List<JobResponse>> jobsToBeScheduled = await _sender.Send(new GetPendingOneTimeAndRecurringJobQuery(alreadyScheduledJobIds), cancellationToken);

        foreach (JobResponse jobResponse in jobsToBeScheduled.Value ?? new())
        {
            List<IJobDetail> jobDetails = CreateJobDetails(jobResponse);
            ConfigureJobChainListener(jobResponse.JobId, jobDetails);
            TriggerBuilder triggerBuilder = GenerateTrigger(jobResponse);
            await _scheduler.ScheduleJob(jobDetails.First(), triggerBuilder.Build(), cancellationToken);
        }
    }

    private void ConfigureJobChainListener(long JobId, List<IJobDetail> jobDetails)
    {
        if (jobDetails.Count > 1)
        {
            JobChainingJobListener listener = new JobChainingJobListener("jobChainListener");
            for (int i = 0; i < jobDetails.Count - 1; i++)
            {
                IJobDetail firstJob = jobDetails[i];
                IJobDetail secondJob = jobDetails[i + 1];
                listener.AddJobChainLink(firstJob.Key, secondJob.Key);
            }
            _scheduler.ListenerManager.AddJobListener(listener, GroupMatcher<JobKey>.GroupEquals($"{JobId}"));
        }
    }

    private List<IJobDetail> CreateJobDetails(JobResponse jobResponse)
    {
        List<IJobDetail> jobDetails = new();

        jobResponse.Steps.ForEach(step =>
        {
            Type jobAssembly = Type.GetType(step.Assembly);

            if (jobAssembly is null)
                throw new InvalidProgramException($"Job assembly {step.Assembly} not found");

            jobDetails.Add(JobBuilder.Create(jobAssembly)
                                       .WithIdentity($"{step.JobStepId}", $"{jobResponse.JobId}")
                                       .UsingJobData("jsonParameter", step.JsonParameter)
                                       .Build());
        });

        return jobDetails;
    }

    private TriggerBuilder GenerateTrigger(JobResponse jobResponse)
    {
        TriggerBuilder triggerBuilder = TriggerBuilder.Create()
                                                   .WithIdentity($"{jobResponse.JobId}")
                                                   .StartAt(jobResponse.EffectiveDateTime);

        string? cronExpression = CronExpressionGenerator(jobResponse.RecurringType,
                                                      jobResponse.Second,
                                                      jobResponse.Minute,
                                                      jobResponse.Hour,
                                                      jobResponse.Day,
                                                      jobResponse.DayOfWeek);

        if (cronExpression is not null)
            triggerBuilder.WithCronSchedule(cronExpression);

        return triggerBuilder;
    }

    private string CronExpressionGenerator(RecurringType? recurringType, int? second, int? minute, int? hour, int? day, DayOfWeek? dayOfWeek)
    {
        if (recurringType is null)
            return null;

        return recurringType switch
        {
            RecurringType.EveryNoSecond => $"0/{second ?? 1} * * * * ?", // Every N seconds
            RecurringType.EveryNoMinute => $"{second ?? 0} 0/{minute ?? 1} * * * ?", // Every N minutes
            RecurringType.Daily => $"{second ?? 0} {minute ?? 0} {hour ?? 0} * * ?", // Every day at a specific time
            RecurringType.Weekly => $"{second ?? 0} {minute ?? 0} {hour ?? 0} ? * {dayOfWeek?.ToString().ToUpper().Substring(0, 3)}", // Every week on a specific day and time
            RecurringType.Monthly => $"{second ?? 0} {minute ?? 0} {hour ?? 0} {day ?? 1} * ?", // Every month on a specific day and time
            _ => throw new NotImplementedException($"Recurring type {recurringType} is not supported")
        };
    }

    public async Task UnSchedule(long GroupId, IEnumerable<long> StepId)
    {
        if (StepId.Any())
        {
            JobKey[] jobsToDelete = StepId.Select(stepId => new JobKey($"{stepId}", $"{GroupId}")).ToArray();
            await _scheduler.DeleteJobs(jobsToDelete);
        }
    }
}
