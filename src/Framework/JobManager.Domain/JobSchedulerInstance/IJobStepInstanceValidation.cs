namespace JobManager.Framework.Domain.JobSchedulerInstance;
public interface IJobStepInstanceValidation
{
    Task<bool> IsValidJobStepInstance(long jobStepInstanceId, CancellationToken cancellationToken=default);
}
