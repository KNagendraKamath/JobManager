using JobManager.Framework.Application.Abstractions.Messaging;

namespace JobManager.Framework.Application.JobSetup.UnscheduleJob;
public record GetJobsToUnscheduleQuery(long[] AlreadyScheduledJobIds) : IQuery<IReadOnlyList<JobGroups>>;

public record JobGroups(long JobId,long JobStepId);
