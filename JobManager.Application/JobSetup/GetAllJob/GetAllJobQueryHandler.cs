using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Application.JobSetup.GetAllJob;
internal sealed class GetAllJobQueryHandler : IQueryHandler<GetAllJobQuery, IEnumerable<Job>>
{
    private readonly IJobRepository _jobRepository;

    public GetAllJobQueryHandler(IJobRepository jobRepository) =>
        _jobRepository = jobRepository;

    public async Task<Result<IEnumerable<Job>>> Handle(GetAllJobQuery request, CancellationToken cancellationToken) => 
        (await _jobRepository.GetJobs(request.Page, request.PageSize, cancellationToken))?.ToList();
}
