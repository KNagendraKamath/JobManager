using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance;
internal sealed class JobInstanceValidation:IJobInstanceValidation
{
    private readonly IJobInstanceRepository _jobInstanceRepository;

    public JobInstanceValidation(IJobInstanceRepository jobInstanceRepository) => _jobInstanceRepository = jobInstanceRepository;

    public async Task<bool> IsValidJobInstance(long jobInstanceId, CancellationToken cancellationToken=default) =>
    (await _jobInstanceRepository.GetByIdAsync(jobInstanceId, cancellationToken)) is not null;
}
