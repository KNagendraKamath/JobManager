using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Application.JobSetup;

internal sealed class JobConfigValidation:IJobConfigValidation
{
    
    private readonly IJobConfigRepository _jobConfigRepository;
    public JobConfigValidation(IJobConfigRepository jobConfigRepository) => _jobConfigRepository = jobConfigRepository;

    public async Task<bool> IsValidJobConfig(string name, CancellationToken cancellationToken=default) =>
        (await _jobConfigRepository.GetJobConfigAsync(name, cancellationToken)) is not null;
}
