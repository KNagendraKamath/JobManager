namespace JobManager.Framework.Domain.JobSetup;

public interface IJobConfigRepository
{
    Task<JobConfig> GetJobConfigAsync(string name, CancellationToken cancellationToken=default);
    Task<IEnumerable<JobConfig>> GetJobConfigByNamesAsync(string namesInCSV, CancellationToken cancellationToken = default);
}
