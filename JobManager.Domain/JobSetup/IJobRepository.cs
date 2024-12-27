namespace JobManager.Domain.JobSetup;
public interface IJobRepository
{
    void Add(Job job);
    Task<Job?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}
