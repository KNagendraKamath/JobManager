namespace JobManager.Domain.JobSetup;
public interface IJobRepository
{
    void Add(Job job);
    Task<Job?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task DeactivateJob(long jobId, CancellationToken cancellationToken = default);
    Task RemoveJobStep(long jobStepId, CancellationToken cancellationToken = default);
}
