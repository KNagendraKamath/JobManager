using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;

namespace JobManager.Application.JobSchedulerInstance.UpdateJobInstance;
internal class UpdateJobInstanceStatusCommandHandler : ICommandHandler<UpdateJobInstanceStatusCommand>
{
    private readonly IJobInstanceRepository _jobInstanceRepository;

    public UpdateJobInstanceStatusCommandHandler(IJobInstanceRepository jobInstanceRepository)
    {
        _jobInstanceRepository = jobInstanceRepository;
    }

    public async Task<Result> Handle(UpdateJobInstanceStatusCommand request, CancellationToken cancellationToken)
    {
        JobInstance jobInstance = await _jobInstanceRepository.GetByIdAsync(request.JobInstanceId, cancellationToken);
        if (jobInstance is null)
            throw new InvalidOperationException($"JobInstance with id {request.JobInstanceId} not found");

        jobInstance.UpdateStatus(request.Status);

        return Result.Success();
    }
}
