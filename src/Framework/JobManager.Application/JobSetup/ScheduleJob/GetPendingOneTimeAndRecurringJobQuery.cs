using JobManager.Framework.Application.Abstractions.Messaging;

namespace JobManager.Framework.Application.JobSetup.ScheduleJob;
public record GetPendingOneTimeAndRecurringJobQuery(long[] AlreadyScheduledJobIds) : IQuery<IReadOnlyList<JobResponse>>;
