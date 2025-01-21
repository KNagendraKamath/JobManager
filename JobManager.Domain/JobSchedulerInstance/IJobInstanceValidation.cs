namespace JobManager.Framework.Domain.JobSchedulerInstance;
public interface IJobInstanceValidation
{
    Task<bool> IsValidJobInstance(long jobInstanceId,CancellationToken cancellationToken=default);
}
