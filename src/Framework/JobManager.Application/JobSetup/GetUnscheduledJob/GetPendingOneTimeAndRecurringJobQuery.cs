using JobManager.Framework.Application.Abstractions.Messaging;

namespace JobManager.Framework.Application.JobSetup.GetJobDetail;
public record GetPendingOneTimeAndRecurringJobQuery(string AlreadyScheduledJobIdsInCsv):IQuery<List<JobResponse>>;
