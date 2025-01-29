using System.Globalization;
using JobManager.Framework.Application.JobSchedulerInstance.CreateInstance;
using JobManager.Framework.Application.JobSchedulerInstance.LogInstance;
using JobManager.Framework.Application.JobSchedulerInstance.UpdateInstance;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSchedulerInstance;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Exceptions = JobManager.Framework.Application.Abstractions.Exceptions;

namespace JobManager.Framework.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;

internal sealed class JobListener : IJobListener
{
    private readonly IServiceProvider _serviceProvider;
    private ISender _sender;
    private long JobId { get; set; }
    private long JobStepId { get; set; }
    private long JobInstanceId { get; set; }
    private long JobStepInstanceId { get; set; }

    public JobListener(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public string Name => "JobListener";

    readonly Func<ISender, long, long, Status,string, Task> updateStatus = static async (_sender, instanceId, stepInstanceId,status, logMessage) =>
    {
        try
        {
            await _sender.Send(new UpdateJobStepInstanceStatusCommand(stepInstanceId, status, DateTime.UtcNow));
            await _sender.Send(new UpdateJobInstanceStatusCommand(instanceId, status));
            await _sender.Send(new LogJobStepInstanceCommand(stepInstanceId, logMessage));
        }
        catch (Exception ex)
        {
            string exceptionDetails = Exceptions.ValidationException.GetExceptionDetails(ex);
            throw new Exception($"An error occurred: {exceptionDetails}");
        }
    };

    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            _sender = scope.ServiceProvider.GetService<ISender>() ?? throw new InvalidOperationException("ISender service not found.");

            JobId = Convert.ToInt64(context.JobDetail.Key.Group, CultureInfo.InvariantCulture);
            JobStepId = Convert.ToInt64(context.JobDetail.Key.Name, CultureInfo.InvariantCulture);
            bool jobInstanceCreated = context.MergedJobDataMap.TryGetLong("JobInstanceId", out long _jobInstanceId);
            JobInstanceId = _jobInstanceId;

            if (!jobInstanceCreated)
            {
                Result<long> jobInstanceResult = await _sender.Send(new CreateJobInstanceCommand(JobId), cancellationToken);
                JobInstanceId = jobInstanceResult.Value;
                context.MergedJobDataMap.Put("JobInstanceId", JobInstanceId);
            }

            Result<long> jobStepInstanceResult = await _sender.Send(new CreateJobStepInstanceCommand(JobInstanceId, JobStepId), cancellationToken);
            JobStepInstanceId = jobStepInstanceResult.Value;

            await updateStatus(_sender,
                               JobInstanceId,
                               JobStepInstanceId,
                               Status.Running,
                               $"Job with Id {JobId} and Step Id {JobStepId} started");
        }
        catch (Exception ex)
        {
            string exceptionDetails = Exceptions.ValidationException.GetExceptionDetails(ex);
            throw new Exception($"An error occurred: {exceptionDetails}");
        }
    }

    public async Task JobWasExecuted(IJobExecutionContext context,
                                     JobExecutionException? jobException,
                                     CancellationToken cancellationToken = default)
    {
        if (jobException is not null)
        { 
            await updateStatus(_sender,
                               JobInstanceId,
                               JobStepInstanceId,
                               Status.CompletedWithErrors,
                               $"Job with Id {JobId} and Step Id {JobStepId} Completed With Errors {jobException.Message}");
            return;
        }
        await updateStatus(_sender,
                           JobInstanceId,
                           JobStepInstanceId,
                           Status.Completed,
                           $"Job with Id {JobId} and Step Id {JobStepId} Completed");
    }
}
