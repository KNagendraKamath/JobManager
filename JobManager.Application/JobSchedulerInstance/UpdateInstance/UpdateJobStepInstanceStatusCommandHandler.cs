using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance.UpdateInstance;
internal class UpdateJobStepInstanceStatusCommandHandler : ICommandHandler<UpdateJobStepInstanceStatusCommand>
{
    private readonly IJobStepInstanceRepository _jobStepInstanceRepository;

    public UpdateJobStepInstanceStatusCommandHandler(IJobStepInstanceRepository jobStepInstanceRepository) => 
        _jobStepInstanceRepository = jobStepInstanceRepository;

    public async Task<Result> Handle(UpdateJobStepInstanceStatusCommand request, CancellationToken cancellationToken)
    {
        JobStepInstance jobStepInstance = await _jobStepInstanceRepository.GetByIdAsync(request.JobStepInstanceId, cancellationToken);

        if (request.Status == Status.Running)
            jobStepInstance!.SetStartTime(DateTimeOffset.UtcNow);

        if (request.Status == Status.Completed)
            jobStepInstance!.SetEndTime(DateTimeOffset.UtcNow);

        return Result.Success();
    }
}
