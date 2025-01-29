using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance.UpdateInstance;
internal sealed class UpdateJobStepInstanceStatusCommandHandler : ICommandHandler<UpdateJobStepInstanceStatusCommand>
{
    private readonly IJobStepInstanceRepository _jobStepInstanceRepository;

    public UpdateJobStepInstanceStatusCommandHandler(IJobStepInstanceRepository jobStepInstanceRepository) => 
        _jobStepInstanceRepository = jobStepInstanceRepository;

    public async Task<Result> Handle(UpdateJobStepInstanceStatusCommand request, CancellationToken cancellationToken)
    {
        JobStepInstance jobStepInstance = (await _jobStepInstanceRepository.GetByIdAsync(request.JobStepInstanceId, cancellationToken))!;

        if (request.Status == Status.Running)
            jobStepInstance.SetStartTime(request.Time,request.Status.ToString());

        if (request.Status == Status.Completed || request.Status == Status.CompletedWithErrors)
            jobStepInstance.SetEndTime(request.Time,request.Status.ToString());

        await _jobStepInstanceRepository.UpdateAsync(jobStepInstance);

        return Result.Success();
    }
}
