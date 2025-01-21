namespace JobManager.Framework.Domain.JobSchedulerInstance;

public interface IJobInstanceRepository
{
    Task AddAsync(JobInstance jobInstance);
    Task UpdateAsync(JobInstance jobInstance);
    Task<JobInstance?> GetByIdAsync(long jobInstanceId,CancellationToken cancellationToken=default);
}
