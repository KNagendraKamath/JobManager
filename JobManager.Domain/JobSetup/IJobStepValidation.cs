namespace JobManager.Framework.Domain.JobSetup;
public interface IJobStepValidation
{
    Task<bool> IsValidJobStep(long jobId, string jobName, CancellationToken cancellationToken = default);
}
