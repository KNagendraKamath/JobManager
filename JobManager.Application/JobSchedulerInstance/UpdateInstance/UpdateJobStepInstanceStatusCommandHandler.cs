using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;
using JobManager.Domain.JobSetup;

namespace JobManager.Application.JobSchedulerInstance.UpdateJobInstance;
internal class UpdateJobStepInstanceStatusCommandHandler : ICommandHandler<UpdateJobStepInstanceStatusCommand>
{
    private readonly IJobStepInstanceRepository _jobStepInstanceRepository;
    private readonly IJobInstanceRepository _jobInstanceRepository;
    private readonly IJobRepository _jobRepository;

    public UpdateJobStepInstanceStatusCommandHandler(IJobStepInstanceRepository jobStepInstanceRepository,
                                                     IJobInstanceRepository jobInstanceRepository,
                                                     IJobRepository jobRepository)
    {
        _jobStepInstanceRepository = jobStepInstanceRepository;
        _jobInstanceRepository = jobInstanceRepository;
        _jobRepository = jobRepository;
    }

    public async Task<Result> Handle(UpdateJobStepInstanceStatusCommand request, CancellationToken cancellationToken)
    {
        JobStepInstance jobStepInstance = await _jobStepInstanceRepository.GetByIdAsync(request.JobStepInstanceId, cancellationToken);
        if (jobStepInstance is null)
            throw new InvalidOperationException($"JobStepInstance with id {request.JobStepInstanceId} not found");

        if (request.Status == Status.Running)
            jobStepInstance.SetStartTime(DateTimeOffset.UtcNow);

        if (request.Status == Status.Completed)
            jobStepInstance.SetEndTime(DateTimeOffset.UtcNow);

        return Result.Success();
    }
}
