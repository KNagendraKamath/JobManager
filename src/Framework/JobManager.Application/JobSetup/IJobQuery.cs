using JobManager.Framework.Application.JobSetup.GetJobDetail;

namespace JobManager.Framework.Application.JobSetup;
public interface IJobQuery
{
    Task<IReadOnlyList<JobResponse>> GetPendingOneTimeAndRecurringJobs(string alreadyScheduledJobIdsInCsv, 
                                                                       CancellationToken cancellationToken = default);
}
