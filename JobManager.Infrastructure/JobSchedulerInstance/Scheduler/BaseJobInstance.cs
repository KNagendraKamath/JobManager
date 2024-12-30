using JobManager.Application.JobSchedulerInstance.CreateJobInstance;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;
using JobManager.Infrastructure.Abstractions;
using MediatR;
using Quartz;

namespace JobManager.Infrastructure.JobSchedulerInstance.Scheduler;

public abstract class BaseJobInstance<TParameter>:IJob
{
    private ISender Sender { get; set; } = ServiceLocator.GetInstance<ISender>();

    protected TParameter Parameter { get; init; }

    public async Task Execute(IJobExecutionContext context)
    {
        long jobId = Convert.ToInt64(context.JobDetail.Key.Group);
        long jobStepId = Convert.ToInt64(context.JobDetail.Key.Name);

        Result<long> jobInstanceResult = (await Sender.Send(new CreateJobInstanceCommand(jobId)));

        //TODO: log the error (gracefully handle the use case)
        if (jobInstanceResult.IsFailure)
            return;

        long jobInstanceId = jobInstanceResult.Value;

        Result<long> jobStepInstanceResult = await Sender.Send(new CreateJobStepInstanceCommand(jobInstanceId, jobStepId));

        //TODO: log the error (gracefully handle the use case)
        if (jobStepInstanceResult.IsFailure)
            return;

        long jobStepInstanceId = jobInstanceResult.Value;

        await UpdateJobStepInstanceStatus(jobStepInstanceId,Status.Running);
        try
        {
            await Execute();
            await UpdateJobStepInstanceStatus(jobStepInstanceId, Status.Completed);

        }
        catch (Exception ex)
        {
            await UpdateJobStepInstanceStatus(jobStepInstanceId, Status.CompletedWithErrors);
            await Sender.Send(new LogJobStepInstanceCommand(jobStepInstanceId, ex.Message));
        }
    }

    private async Task UpdateJobStepInstanceStatus(long jobStepInstanceId,Status status)
    {
        await Sender.Send(new UpdateJobStepInstanceStatusCommand(jobStepInstanceId, Status.Running, DateTimeOffset.Now));
    }

    public abstract Task Execute();



}
