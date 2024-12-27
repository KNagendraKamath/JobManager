namespace JobManager.Domain.JobSetup;

public interface IJobConfigRepository
{
    Task<JobConfig> GetJobConfigAsync(string name, CancellationToken cancellationToken=default);
}
