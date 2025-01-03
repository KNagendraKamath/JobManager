using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;
using MediatR;

namespace JobManager.Application.JobSchedulerInstance.CreateJobInstance;
internal class CreateJobStepInstanceCommandHandler : ICommandHandler<CreateJobStepInstanceCommand,long>
{
    private readonly IJobInstanceRepository _jobInstanceRepository;
    private readonly IUnitOfWork _unitofWork;

    public CreateJobStepInstanceCommandHandler(IJobInstanceRepository jobInstanceRepository, IUnitOfWork unitofWork)
    {
        _jobInstanceRepository = jobInstanceRepository;
        _unitofWork = unitofWork;
    }

    async Task<Result<long>> IRequestHandler<CreateJobStepInstanceCommand, Result<long>>.Handle(CreateJobStepInstanceCommand request, CancellationToken cancellationToken)
    {
        JobInstance? jobInstance = await _jobInstanceRepository.GetByIdAsync(request.JobInstanceId, cancellationToken);

        if (jobInstance is null)
            throw new InvalidOperationException($"JobInstance with Id {request.JobInstanceId} not found");

        JobStepInstance jobStepInstance = JobStepInstance.Create(jobInstance.Id, request.JobStepId, Status.NotStarted);
        jobInstance.AddJobStepInstance(jobStepInstance);
        _jobInstanceRepository.Update(jobInstance);
        await _unitofWork.SaveChangesAsync(cancellationToken);
        return jobStepInstance.Id;

    }
}
