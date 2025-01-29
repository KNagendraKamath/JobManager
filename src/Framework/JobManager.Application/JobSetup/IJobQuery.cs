using JobManager.Framework.Application.JobSetup.ScheduleJob;
using JobManager.Framework.Application.JobSetup.UnscheduleJob;

namespace JobManager.Framework.Application.JobSetup;
public interface IJobQuery
{
    Task<IReadOnlyList<JobResponse>> GetPendingOneTimeAndRecurringActiveJobs(long[] alreadyScheduledJobIds, 
                                                                       CancellationToken cancellationToken = default);

    Task<IReadOnlyList<JobGroups>> GetJobsToUnschedule(long[] alreadyScheduledJobIds,
                                                                   CancellationToken cancellationToken = default);
}
