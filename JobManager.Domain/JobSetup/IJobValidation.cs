namespace JobManager.Framework.Domain.JobSetup;
public interface IJobValidation
{
    Task<bool> IsValidJob(long jobId, CancellationToken cancellationToken=default);
}
