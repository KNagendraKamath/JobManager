using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;
using MediatR;

namespace JobManager.Application.JobSchedulerInstance.CreateJobInstance;
internal class CreateJobStepInstanceCommandHandler : ICommandHandler<CreateJobStepInstanceCommand,long>
{
    private readonly IJobInstanceRepository _jobInstanceRepository;

    public CreateJobStepInstanceCommandHandler(IJobInstanceRepository jobInstanceRepository)
    {
        _jobInstanceRepository = jobInstanceRepository;
    
    }

    async Task<Result<long>> IRequestHandler<CreateJobStepInstanceCommand, Result<long>>.Handle(CreateJobStepInstanceCommand request, CancellationToken cancellationToken)
    {
        JobInstance? jobInstance = await _jobInstanceRepository.GetByIdAsync(request.JobInstanceId, cancellationToken);

        if (jobInstance is null)
            throw new InvalidOperationException($"JobInstance with Id {request.JobInstanceId} not found");

        JobStepInstance jobStepInstance = JobStepInstance.Create(jobInstance.Id, request.JobStepId, Status.NotStarted);
        jobInstance.AddJobStepInstance(jobStepInstance);
        await _jobInstanceRepository.UpdateAsync(jobInstance);
        return jobStepInstance.Id;

    }
}
