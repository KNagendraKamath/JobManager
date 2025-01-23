using JobManager.Framework.Application.JobSetup.GetJobDetail;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSchedulerInstance;
using MediatR;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Listener;

namespace JobManager.Framework.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;

internal sealed class Scheduler : IJobScheduler
{
    private readonly ISchedulerFactory _schedulerFactory;
    private IScheduler _scheduler;
    private readonly ISender _sender;

    public Scheduler(ISchedulerFactory schedulerFactory,
                           ISender sender
                           )
    {
        _schedulerFactory = schedulerFactory;
        _sender = sender;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken=default)
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
            await _scheduler.ScheduleJob(jobDetails[0], triggerBuilder.Build(), cancellationToken);
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
