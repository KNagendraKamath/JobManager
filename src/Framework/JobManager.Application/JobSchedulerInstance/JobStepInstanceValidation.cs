using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance;
internal sealed class JobStepInstanceValidation:IJobStepInstanceValidation
{
    private readonly IJobStepInstanceRepository _jobStepInstanceRepository;

    public JobStepInstanceValidation(IJobStepInstanceRepository jobStepInstanceRepository) => _jobStepInstanceRepository = jobStepInstanceRepository;

    public async Task<bool> IsValidJobStepInstance(long jobStepInstanceId, CancellationToken cancellationToken=default) =>
    (await _jobStepInstanceRepository.GetByIdAsync(jobStepInstanceId, cancellationToken)) is not null;
}
