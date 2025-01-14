using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;

namespace JobManager.Application.JobSchedulerInstance.CreateJobInstance;
internal class LogJobStepInstanceCommandHandler : ICommandHandler<LogJobStepInstanceCommand>
{
    private readonly IJobStepInstanceRepository _jobStepInstanceRepository;


    public LogJobStepInstanceCommandHandler(IJobStepInstanceRepository jobStepInstanceRepository)
    {
        _jobStepInstanceRepository = jobStepInstanceRepository;
    }


    public async Task<Result> Handle(LogJobStepInstanceCommand request, CancellationToken cancellationToken)
    {
        JobStepInstance? jobStepInstance = await _jobStepInstanceRepository.GetByIdAsync(request.JobStepInstanceId, cancellationToken);

        if (jobStepInstance is null)
            throw new InvalidOperationException($"JobStepInstance with Id {request.JobStepInstanceId} not found");

        jobStepInstance.AddLog(request.Message);
        await _jobStepInstanceRepository.UpdateAsync(jobStepInstance);
        return Result.Success();

    }
}
