namespace JobManager.Domain.JobSchedulerInstance;

public interface IJobStepInstanceRepository
{
    void Update(JobStepInstance jobStepInstance);
    Task<JobStepInstance?> GetByIdAsync(long Id, CancellationToken cancellationToken);
}
