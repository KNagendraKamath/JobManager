using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance.CreateInstance;
internal sealed class CreateJobStepInstanceCommandHandler : ICommandHandler<CreateJobStepInstanceCommand,long>
{
    private readonly IJobInstanceRepository _jobInstanceRepository;

    public CreateJobStepInstanceCommandHandler(IJobInstanceRepository jobInstanceRepository) => 
        _jobInstanceRepository = jobInstanceRepository;

    public async Task<Result<long>> Handle(CreateJobStepInstanceCommand request, CancellationToken cancellationToken)
    {
        JobInstance jobInstance = await _jobInstanceRepository.GetByIdAsync(request.JobInstanceId, cancellationToken);
        JobStepInstance jobStepInstance = JobStepInstance.Create(jobInstance!.Id, request.JobStepId, Status.NotStarted);
        jobInstance.AddJobStepInstance(jobStepInstance);
        await _jobInstanceRepository.UpdateAsync(jobInstance);
        return jobStepInstance.Id;
    }
}
