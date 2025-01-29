using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.Abstractions;

namespace JobManager.Framework.Application.JobSetup.ScheduleJob;

internal sealed class GetPendingOneTimeAndRecurringJobQueryHandler : IQueryHandler<GetPendingOneTimeAndRecurringJobQuery, IReadOnlyList<JobResponse>>
{
    private readonly IJobQuery _jobQuery;

    public GetPendingOneTimeAndRecurringJobQueryHandler(IJobQuery jobQuery) =>
        _jobQuery = jobQuery;

    public async Task<Result<IReadOnlyList<JobResponse>>> Handle(GetPendingOneTimeAndRecurringJobQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<JobResponse> jobs = await _jobQuery.GetPendingOneTimeAndRecurringActiveJobs(request.AlreadyScheduledJobIds, cancellationToken);
        return Result.Success(jobs);
    }
}
