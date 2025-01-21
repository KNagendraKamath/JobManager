namespace JobManager.Framework.Domain.JobSchedulerInstance;
public interface IJobScheduler
{
    Task UnSchedule(long GroupId,IEnumerable<long> StepId);

    Task ExecuteAsync(CancellationToken cancellationToken = default);
}
