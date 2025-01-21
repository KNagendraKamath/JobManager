using JobManager.Framework.Application.JobSchedulerInstance.CreateInstance;
using JobManager.Framework.Application.JobSchedulerInstance.UpdateInstance;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSchedulerInstance;
using MediatR;
using Quartz;

namespace JobManager.Framework.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;

internal class JobListener : IJobListener
{
    public JobListener(ISender sender)
    {
        _sender = sender;
    }
    private readonly ISender _sender;
    private long JobId { get; set; }
    private long JobStepId { get; set; }
    private long JobInstanceId { get; set; }
    private long JobStepInstanceId { get; set; }

    public string Name => "JobListener";

    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        
        throw new NotImplementedException();
    }

    public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        JobId = Convert.ToInt64(context.JobDetail.Key.Group);
        JobStepId = Convert.ToInt64(context.JobDetail.Key.Name);
        bool jobInstanceCreated=context.MergedJobDataMap.TryGetLong("JobInstanceId", out long _jobInstanceId);
        JobInstanceId = _jobInstanceId;

        if (!jobInstanceCreated)
        {
            Result<long> jobInstanceResult = await _sender.Send(new CreateJobInstanceCommand(JobId), cancellationToken);
            JobInstanceId = jobInstanceResult.Value;
            context.MergedJobDataMap.Put("JobInstanceId", JobInstanceId);
        }
        
        Result<long> jobStepInstanceResult = await _sender.Send(new CreateJobStepInstanceCommand(JobInstanceId, JobStepId));

        JobStepInstanceId = jobStepInstanceResult.Value;
        await UpdateInstanceStatus(JobInstanceId, JobStepInstanceId, Status.Running);
    }

    public async Task JobWasExecuted(IJobExecutionContext context,
                                     JobExecutionException? jobException,
                                     CancellationToken cancellationToken = default)
    {
        if (jobException is not null)
        {
            await UpdateInstanceStatus(JobInstanceId, JobStepInstanceId, Status.CompletedWithErrors);
            await _sender.Send(new LogJobStepInstanceCommand(JobStepInstanceId, jobException.Message));
            return;
        }
        await UpdateJobStepInstanceStatus(JobStepInstanceId, Status.Completed);
    }

    private async Task UpdateInstanceStatus(long jobInstanceId,
                                            long jobStepInstanceId,
                                            Status status)
    {
        await UpdateJobStepInstanceStatus(jobStepInstanceId, status);
        await UpdateJobInstanceStatus(jobInstanceId, status);
    }

    private async Task UpdateJobInstanceStatus(long jobInstanceId, Status status) =>
        await _sender.Send(new UpdateJobInstanceStatusCommand(jobInstanceId, status));

    private async Task UpdateJobStepInstanceStatus(long jobStepInstanceId, Status status) =>
        await _sender.Send(new UpdateJobStepInstanceStatusCommand(jobStepInstanceId, Status.Running, DateTimeOffset.Now));

}
