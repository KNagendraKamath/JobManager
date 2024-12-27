using JobManager.Application.Abstractions.Messaging;

namespace JobManager.Application.JobSetup.GetJobDetail;
public record GetPendingOneTimeAndRecurringJobQuery:IQuery<List<JobResponse>>;
