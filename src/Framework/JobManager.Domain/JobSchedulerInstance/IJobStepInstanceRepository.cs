namespace JobManager.Framework.Domain.JobSchedulerInstance;

public interface IJobStepInstanceRepository
{
    Task<JobStepInstance> AddAsync(JobStepInstance jobStepInstance);
    Task<bool> UpdateAsync(JobStepInstance jobStepInstance);
    Task<JobStepInstance?> GetByIdAsync(long Id, CancellationToken cancellationToken);
}
