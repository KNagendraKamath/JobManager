using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance.CreateInstance;
internal sealed class CreateJobStepInstanceCommandHandler : ICommandHandler<CreateJobStepInstanceCommand,long>
{
    private readonly IJobStepInstanceRepository _jobStepInstanceRepository;

    public CreateJobStepInstanceCommandHandler(IJobStepInstanceRepository jobStepInstanceRepository) => 
        _jobStepInstanceRepository = jobStepInstanceRepository;

    public async Task<Result<long>> Handle(CreateJobStepInstanceCommand request, CancellationToken cancellationToken)
    {
        JobStepInstance jobStepInstance = JobStepInstance.Create(request.JobInstanceId, request.JobStepId, Status.NotStarted.ToString());
        await _jobStepInstanceRepository.AddAsync(jobStepInstance);
        return jobStepInstance.Id;
    }
}
