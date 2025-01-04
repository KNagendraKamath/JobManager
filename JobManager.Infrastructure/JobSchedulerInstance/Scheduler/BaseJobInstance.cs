using System.Text.Json;
using JobManager.Application.JobSchedulerInstance.CreateJobInstance;
using JobManager.Application.JobSchedulerInstance.UpdateJobInstance;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace JobManager.Infrastructure.JobSchedulerInstance.Scheduler;

[DisallowConcurrentExecution]
public abstract class BaseJobInstance<TParameter>:IJob
{
    public BaseJobInstance(IServiceProvider serviceProvider)
    {
        Sender = serviceProvider.GetRequiredService<ISender>();
    }
    private readonly ISender Sender;

    protected TParameter? Parameter { get; private set; }

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

        await UpdateInstanceStatus(jobInstanceId, jobStepInstanceId,Status.Running);

        try
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Parameter = JsonSerializer.Deserialize<TParameter>(dataMap.GetString("jsonParameter") ?? "{}");
            await Execute();
            await UpdateInstanceStatus(jobInstanceId, jobStepInstanceId, Status.Completed);
        }
        catch (Exception ex)
        {
            await UpdateInstanceStatus(jobInstanceId, jobStepInstanceId, Status.CompletedWithErrors);
            await Sender.Send(new LogJobStepInstanceCommand(jobStepInstanceId, ex.Message));
        }
    }

    private async Task UpdateInstanceStatus(long jobInstanceId, long jobStepInstanceId,Status status)
    {
        await UpdateJobStepInstanceStatus(jobStepInstanceId, status);
        await UpdateJobInstanceStatus(jobInstanceId, status);
    }

    private async Task UpdateJobInstanceStatus(long jobInstanceId, Status status)
    {
        await Sender.Send(new UpdateJobInstanceStatusCommand(jobInstanceId, status));
    }

    private async Task UpdateJobStepInstanceStatus(long jobStepInstanceId,Status status)
    {
        await Sender.Send(new UpdateJobStepInstanceStatusCommand(jobStepInstanceId, Status.Running, DateTimeOffset.Now));
    }

    public abstract Task Execute();



}
