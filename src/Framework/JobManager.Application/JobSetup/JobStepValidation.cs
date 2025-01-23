using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Application.JobSetup;
internal sealed class JobStepValidation:IJobStepValidation
{
    private readonly IJobStepRepository _jobStepRepository;

    public JobStepValidation(IJobStepRepository jobStepRepository)
        => _jobStepRepository = jobStepRepository;

    public async Task<bool> IsValidJobStep(long jobId, string jobName, CancellationToken cancellationToken=default) 
        => (await _jobStepRepository.GetJobStep(jobId, jobName, cancellationToken)) is not null;

}
