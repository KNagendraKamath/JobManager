using System.Threading;
using JobManager.Framework.Application.JobSetup.GetJobDetail;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSchedulerInstance;
using JobManager.Framework.Domain.JobSetup;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Listener;

namespace JobManager.Framework.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;

internal sealed class Scheduler : IJobScheduler
{
    private readonly IScheduler _scheduler;
    private readonly IServiceProvider _serviceProvider;
    private readonly IJobAssemblyProvider _jobAssemblyProvider;
    private readonly ILogger<Scheduler> _logger;

    public Scheduler(ISchedulerFactory schedulerFactory,
                     IJobAssemblyProvider jobAssemblyProvider,
                     ILogger<Scheduler> logger,
                     IServiceProvider serviceProvider
                    )
    {
        _jobAssemblyProvider = jobAssemblyProvider;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _scheduler = schedulerFactory.GetScheduler().Result;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken=default)
    {
        _logger.LogInformation("Fetching any new jobs to be scheduled");

        IReadOnlyCollection<JobKey> jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup(), cancellationToken);
        string alreadyScheduledJobIds = string.Join(",", jobKeys.Select(j => j.Group));

        using IServiceScope scope = _serviceProvider.CreateScope();
        Result<IReadOnlyList<JobResponse>> jobsToBeScheduled = await scope.ServiceProvider
                                                                 .GetRequiredService<ISender>()
                                                                 .Send(new GetPendingOneTimeAndRecurringJobQuery(alreadyScheduledJobIds),
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
            await _scheduler.ScheduleJob(jobDetails[0], triggerBuilder.Build(), cancellationToken);
        }
        _logger.LogInformation("New jobs scheduled");
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
                throw new InvalidProgramException($"Job assembly {jobAssemblyName} not found");

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


    public async Task UnSchedule(long GroupId, List<long> StepId)
    {
        if (StepId.Any())
        {
            JobKey[] jobsToDelete = StepId.Select(stepId => new JobKey($"{stepId}", $"{GroupId}")).ToArray();
            await _scheduler.DeleteJobs(jobsToDelete);
        }
    }
}
