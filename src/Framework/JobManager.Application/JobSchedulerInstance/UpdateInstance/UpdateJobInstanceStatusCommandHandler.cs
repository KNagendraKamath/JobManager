using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance.UpdateInstance;

internal sealed class UpdateJobInstanceStatusCommandHandler : ICommandHandler<UpdateJobInstanceStatusCommand>
{
    private readonly IJobInstanceRepository _jobInstanceRepository;

    public UpdateJobInstanceStatusCommandHandler(IJobInstanceRepository jobInstanceRepository) => 
        _jobInstanceRepository = jobInstanceRepository;

    public async Task<Result> Handle(UpdateJobInstanceStatusCommand request, CancellationToken cancellationToken)
    {
        JobInstance jobInstance = await _jobInstanceRepository.GetByIdAsync(request.JobInstanceId, cancellationToken);
        jobInstance!.UpdateStatus(request.Status);
        await _jobInstanceRepository.UpdateAsync(jobInstance);
        return Result.Success();
    }
}
