using JobManager.Application.Abstractions.Messaging;

namespace JobManager.Application.JobSetup.GetJobDetail;
public record GetPendingOneTimeAndRecurringJobQuery(string ScheduledJobIdsInCsv):IQuery<List<JobResponse>>;
