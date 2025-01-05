using JobManager.Application.Abstractions.Messaging;

namespace JobManager.Application.JobSetup.GetJobDetail;
public record GetPendingOneTimeAndRecurringJobQuery(string AlreadyScheduledJobIdsInCsv):IQuery<List<JobResponse>>;
