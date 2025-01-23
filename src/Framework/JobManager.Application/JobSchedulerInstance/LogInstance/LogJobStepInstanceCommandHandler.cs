using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance.LogInstance;
internal sealed class LogJobStepInstanceCommandHandler : ICommandHandler<LogJobStepInstanceCommand>
{
    private readonly IJobStepInstanceRepository _jobStepInstanceRepository;


    public LogJobStepInstanceCommandHandler(IJobStepInstanceRepository jobStepInstanceRepository) =>
        _jobStepInstanceRepository = jobStepInstanceRepository;

    public async Task<Result> Handle(LogJobStepInstanceCommand request, CancellationToken cancellationToken)
    {
        JobStepInstance jobStepInstance = await _jobStepInstanceRepository.GetByIdAsync(request.JobStepInstanceId, cancellationToken);
        jobStepInstance!.AddLog(request.Message);
        await _jobStepInstanceRepository.UpdateAsync(jobStepInstance);
        return Result.Success();
    }
}
