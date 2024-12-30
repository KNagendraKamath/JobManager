namespace JobManager.Domain.JobSchedulerInstance;

public interface IJobInstanceRepository
{
    void Add(JobInstance jobInstance);
    void Update(JobInstance jobInstance);
    Task<JobInstance?> GetByIdAsync(long jobInstanceId,CancellationToken cancellationToken);
}
