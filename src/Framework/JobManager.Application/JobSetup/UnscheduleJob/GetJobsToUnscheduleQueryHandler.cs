using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.Abstractions;

namespace JobManager.Framework.Application.JobSetup.UnscheduleJob;
internal sealed class GetJobsToUnscheduleQueryHandler : IQueryHandler<GetJobsToUnscheduleQuery, IReadOnlyList<JobGroups>>
{
    private readonly IJobQuery _jobQuery;

    public GetJobsToUnscheduleQueryHandler(IJobQuery jobQuery) =>
        _jobQuery = jobQuery;

    public async Task<Result<IReadOnlyList<JobGroups>>> Handle(GetJobsToUnscheduleQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<JobGroups> jobs = await _jobQuery.GetJobsToUnschedule(request.AlreadyScheduledJobIds, cancellationToken);
        return Result.Success(jobs);
    }
}
