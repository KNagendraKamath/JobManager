using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Application.JobSetup;

internal sealed class JobConfigValidation:IJobConfigValidation
{
    private readonly IJobConfigRepository _jobConfigRepository;
    internal JobConfigValidation(IJobConfigRepository jobConfigRepository) => _jobConfigRepository = jobConfigRepository;

    public async Task<bool> IsValidJobConfig(string name, CancellationToken cancellationToken) =>
        (await _jobConfigRepository.GetJobConfigAsync(name, cancellationToken)) is not null;
}
