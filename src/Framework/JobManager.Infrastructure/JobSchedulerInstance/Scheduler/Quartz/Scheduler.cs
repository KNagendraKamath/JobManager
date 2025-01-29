using System.Globalization;
using JobManager.Framework.Application.JobSetup.ScheduleJob;
using JobManager.Framework.Application.JobSetup.UnscheduleJob;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSetup;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Listener;
using Exceptions = JobManager.Framework.Application.Abstractions.Exceptions;

namespace JobManager.Framework.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;

internal sealed class Scheduler : IJobScheduler
{
    private readonly IScheduler _scheduler;

    private readonly ILogger<Scheduler> _logger;
    private ISender _sender;
    private IJobAssemblyProvider _jobAssemblyProvider;

    public Scheduler(ISchedulerFactory schedulerFactory,
                     ILogger<Scheduler> logger)
    {
        _logger = logger;
        _scheduler = schedulerFactory.GetScheduler().Result;

    }

    public async Task ExecuteAsync(IServiceScope scope, CancellationToken cancellationToken = default)
    {
        try
        {
            _sender = scope.ServiceProvider.GetRequiredService<ISender>();
            _jobAssemblyProvider = scope.ServiceProvider.GetRequiredService<IJobAssemblyProvider>();

            _logger.LogInformation("Fetching any new jobs to be scheduled");

            IReadOnlyCollection<JobKey> jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup(), cancellationToken);
            long[] alreadyScheduledJobIds = jobKeys.Select(j => Convert.ToInt64(j.Group, CultureInfo.InvariantCulture)).ToArray();

            await UnscheduleJobAsync(alreadyScheduledJobIds, cancellationToken);
            await ScheduleJobAsync(alreadyScheduledJobIds, cancellationToken);
        }
        catch (Exception ex)
        {
            string exceptionDetails = Exceptions.ValidationException.GetExceptionDetails(ex);
            _logger.LogError(ex, "An error occurred: {ExceptionDetails}", exceptionDetails);
        }

    }

    private async Task ScheduleJobAsync(long[] alreadyScheduledJobIds,
                                    CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<JobResponse>> jobsToBeScheduled = await _sender.Send(new GetPendingOneTimeAndRecurringJobQuery(alreadyScheduledJobIds),
                                                                               cancellationToken);

        if (jobsToBeScheduled.IsFailure)
        {
            _logger.LogError("Failed to retrieve jobs to be scheduled: {ErrorDescription}", jobsToBeScheduled.Error.Description);
            return;
        }

        foreach (JobResponse jobResponse in jobsToBeScheduled.Value)
        {
            List<IJobDetail> jobDetails = CreateJobDetails(jobResponse);
            ConfigureJobChainListener(jobResponse.JobId, jobDetails);
            TriggerBuilder triggerBuilder = GenerateTrigger(jobResponse);
            if (await _scheduler.CheckExists(jobDetails[0].Key, cancellationToken))
            {
                _logger.LogInformation("Job {JobId} is already scheduled. Skipping...", jobResponse.JobId);
                continue;
            }
            await _scheduler.ScheduleJob(jobDetails[0], triggerBuilder.Build(), cancellationToken);
        }
    }

    private async Task UnscheduleJobAsync(long[] alreadyScheduledJobIds,
                                      CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<JobGroups>> jobsToBeUnscheduled = await _sender.Send(new GetJobsToUnscheduleQuery(alreadyScheduledJobIds),
                                                                      cancellationToken);

        if (jobsToBeUnscheduled.IsFailure)
        {
            _logger.LogError("Failed to retrieve jobs to be scheduled: {ErrorDescription}", jobsToBeUnscheduled.Error.Description);
            return;
        }

        await UnSchedule(jobsToBeUnscheduled.Value);
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
            string jobAssemblyName = _jobAssemblyProvider.GetAssemblyName(step.JobConfigName);
            Type jobAssembly = Type.GetType(jobAssemblyName);

            if (jobAssembly is null)
            {
                _logger.LogError("Job assembly {JobAssemblyName} not found", jobAssemblyName);
                return;
            }

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

        if (jobResponse.CronExpression is not null)
            triggerBuilder.WithCronSchedule(jobResponse.CronExpression);

        return triggerBuilder;
    }

    public async Task UnSchedule(IReadOnlyList<JobGroups> jobGroups)
    {
        if (jobGroups.Any())
        {
            JobKey[] jobsToDelete = jobGroups.Select(job => new JobKey($"{job.JobStepId}", $"{job.JobId}")).ToArray();
            await _scheduler.DeleteJobs(jobsToDelete);
        }
    }
}
