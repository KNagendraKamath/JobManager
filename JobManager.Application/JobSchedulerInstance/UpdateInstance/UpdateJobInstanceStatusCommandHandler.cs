using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;

namespace JobManager.Application.JobSchedulerInstance.UpdateJobInstance;
internal class UpdateJobInstanceStatusCommandHandler : ICommandHandler<UpdateJobInstanceStatusCommand>
{
    private readonly IJobInstanceRepository _jobInstanceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateJobInstanceStatusCommandHandler(IUnitOfWork unitOfWork, IJobInstanceRepository jobInstanceRepository)
    {
        _unitOfWork = unitOfWork;
        _jobInstanceRepository = jobInstanceRepository;
    }

    public async Task<Result> Handle(UpdateJobInstanceStatusCommand request, CancellationToken cancellationToken)
    {
        JobInstance jobInstance = await _jobInstanceRepository.GetByIdAsync(request.JobInstanceId, cancellationToken);
        if (jobInstance is null)
            throw new InvalidOperationException($"JobInstance with id {request.JobInstanceId} not found");

        jobInstance.UpdateStatus(request.Status);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
