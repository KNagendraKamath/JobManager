using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Application.JobSetup;
internal sealed class JobValidation:IJobValidation
{
    private readonly IJobRepository _jobRepository;

    public JobValidation(IJobRepository jobRepository) => _jobRepository = jobRepository;

    public async Task<bool> IsValidJob(long jobId, CancellationToken cancellationToken= default) =>
        (await _jobRepository.GetByIdAsync(jobId, cancellationToken)) is not null;

}
