using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;

namespace JobManager.Application.JobSchedulerInstance.CreateJobInstance;
internal class UpdateJobStepInstanceStatusCommandHandler : ICommandHandler<UpdateJobStepInstanceStatusCommand>
{
    private readonly IJobStepInstanceRepository _jobStepInstanceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateJobStepInstanceStatusCommandHandler(IJobStepInstanceRepository jobStepInstanceRepository, IUnitOfWork unitOfWork)
    {
        _jobStepInstanceRepository = jobStepInstanceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateJobStepInstanceStatusCommand request, CancellationToken cancellationToken)
    {
       JobStepInstance jobStepInstance = await _jobStepInstanceRepository.GetByIdAsync(request.JobStepInstanceId,cancellationToken);

        if(jobStepInstance is null)
            throw new InvalidOperationException($"JobStepInstance with id {request.JobStepInstanceId} not found");

        if (request.Status == Status.Running)
            jobStepInstance.SetStartTime(DateTimeOffset.UtcNow);

        if (request.Status == Status.Completed)
            jobStepInstance.SetEndTime(DateTimeOffset.UtcNow);

        _jobStepInstanceRepository.Update(jobStepInstance);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
