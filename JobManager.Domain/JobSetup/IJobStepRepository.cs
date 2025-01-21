
namespace JobManager.Framework.Domain.JobSetup;
public interface IJobStepRepository
{
    Task<JobStep?> GetJobStep(long jobId, string jobName, CancellationToken cancellationToken);
}
