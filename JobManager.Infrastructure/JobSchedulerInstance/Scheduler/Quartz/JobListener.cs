using JobManager.Application.JobSchedulerInstance.CreateJobInstance;
using JobManager.Application.JobSchedulerInstance.UpdateJobInstance;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;
using MediatR;
using Quartz;

namespace JobManager.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;
internal class JobListener : IJobListener
{
    public JobListener(ISender sender)
    {
        _sender = sender;
    }
    private readonly ISender _sender;
    private long JobInstanceId { get; set; }
    private long JobStepInstanceId { get; set; }

    public string Name => "JobListener";

    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        long jobId = Convert.ToInt64(context.JobDetail.Key.Group);
        long jobStepId = Convert.ToInt64(context.JobDetail.Key.Name);

        Result<long> jobInstanceResult = await _sender.Send(new CreateJobInstanceCommand(jobId), cancellationToken);
        JobInstanceId = jobInstanceResult.Value;
        Result<long> jobStepInstanceResult = await _sender.Send(new CreateJobStepInstanceCommand(JobInstanceId, jobStepId));

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
        }
        await UpdateInstanceStatus(JobInstanceId, JobStepInstanceId, Status.Completed);
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
