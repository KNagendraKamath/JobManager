namespace JobManager.Framework.Domain.JobSetup;

public interface IJobConfigValidation
{
    Task<bool> IsValidJobConfig(string name, CancellationToken cancellationToken = default);
}
