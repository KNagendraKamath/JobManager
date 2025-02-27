﻿using JobManager.Application.JobSetup.GetJobDetail;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSetup;
using MediatR;
using Quartz.Impl.Matchers;
using Quartz;
using Microsoft.Extensions.Logging;
using JobManager.Domain.JobSchedulerInstance;

namespace JobManager.Infrastructure.JobSchedulerInstance.Scheduler;

internal class QuartzScheduler : IJobScheduler
{
    private readonly ISchedulerFactory SchedulerFactory;
    private IScheduler Scheduler { get; set; }
    private readonly ISender Sender;
    private readonly ILogger<QuartzScheduler> _logger;

   public QuartzScheduler(ISchedulerFactory schedulerFactory,
                          ISender sender,
                          ILogger<QuartzScheduler> logger)
    {
        SchedulerFactory = schedulerFactory;
        Sender = sender;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        Scheduler = await SchedulerFactory.GetScheduler(cancellationToken);
        await ScheduleJobsFromDatabase(cancellationToken);
    }

    private async Task ScheduleJobsFromDatabase(CancellationToken cancellationToken)
    {
        IReadOnlyCollection<JobKey> jobKeys = await Scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup(), cancellationToken);
        string alreadyScheduledJobIds = string.Join(",", jobKeys.Select(j => j.Group));

        Result<List<JobResponse>> jobsToBeScheduled = await Sender.Send(new GetPendingOneTimeAndRecurringJobQuery(alreadyScheduledJobIds), cancellationToken);

        foreach (JobResponse jobResponse in jobsToBeScheduled.Value ?? new())
        {
            string? cronExpression = CronExpressionGenerator(jobResponse.RecurringType,
                                                             jobResponse.Second,
                                                             jobResponse.Minute,
                                                             jobResponse.Hour,
                                                             jobResponse.Day,
                                                             jobResponse.DayOfWeek);

            jobResponse.Steps.ForEach(async step =>
            {
                await ScheduleAsync(jobResponse.JobId,
                               step.JobStepId,
                               step.Assembly,
                               step.JsonParameter,
                               jobResponse.EffectiveDateTime,
                               cronExpression,
                               cancellationToken);
            });
        }

    }

    private string CronExpressionGenerator(RecurringType? recurringType, int? second, int? minute, int? hour, int? day, DayOfWeek? dayOfWeek)
    {
        if (recurringType is null)
            return null;

        return recurringType switch
        {
            RecurringType.EveryNoSecond => $"0/{second ?? 1} * * * * ?", // Every N seconds
            RecurringType.EveryNoMinute => $"{second ?? 0} 0/{minute ?? 1} * * * ?", // Every N minutes
            RecurringType.Daily => $"{second??0} {minute ?? 0} {hour ?? 0} * * ?", // Every day at a specific time
            RecurringType.Weekly => $"{second ?? 0} {minute ?? 0} {hour ?? 0} ? * {dayOfWeek?.ToString().ToUpper().Substring(0, 3)}", // Every week on a specific day and time
            RecurringType.Monthly => $"{second ?? 0} {minute ?? 0} {hour ?? 0} {day ?? 1} * ?", // Every month on a specific day and time
            _ => throw new NotImplementedException($"Recurring type {recurringType} is not supported")
        };
    }

    public async Task ScheduleAsync(long GroupId,
                         long StepId,
                         string Assembly,
                         string JsonParameter,
                         DateTime EffectiveDateTime,
                         string? CronExpression=default,
                         CancellationToken cancellationToken=default)
    {
        Type jobAssembly = Type.GetType(Assembly);

        if (jobAssembly is null)
        {
            _logger.LogError($"Job assembly {Assembly} not found");
            return;
        }

        IJobDetail job = JobBuilder.Create(jobAssembly)
                                   .WithIdentity($"{StepId}", $"{GroupId}")
                                   .UsingJobData("jsonParameter", JsonParameter)
                                   .Build();

        TriggerBuilder triggerBuilder = TriggerBuilder.Create()
                                .WithIdentity($"{StepId}", $"{GroupId}")
                               .StartAt(EffectiveDateTime);

        if (CronExpression is not null)
            triggerBuilder.WithCronSchedule(CronExpression);

        await Scheduler.ScheduleJob(job, triggerBuilder.Build(), cancellationToken);
    }

    public async Task UnSchedule(long GroupId, IEnumerable<long> StepId)
    {
        if (StepId.Any())
        {
            JobKey[] jobsToDelete = StepId.Select(stepId => new JobKey($"{stepId}", $"{GroupId}")).ToArray();
            await Scheduler.DeleteJobs(jobsToDelete);
        }
    }
}
