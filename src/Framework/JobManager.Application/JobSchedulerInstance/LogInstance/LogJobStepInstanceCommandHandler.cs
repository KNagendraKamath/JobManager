using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance.LogInstance;
internal sealed class LogJobStepInstanceCommandHandler : ICommandHandler<LogJobStepInstanceCommand>
{
    private readonly IJobStepInstanceLogRepository _jobStepInstanceLogRepository;

    public LogJobStepInstanceCommandHandler(IJobStepInstanceLogRepository jobStepInstanceLogRepository) =>
        _jobStepInstanceLogRepository = jobStepInstanceLogRepository;

    public async Task<Result> Handle(LogJobStepInstanceCommand request, CancellationToken cancellationToken)
    {
        JobStepInstanceLog jobStepInstanceLog = JobStepInstanceLog.Create(request.JobStepInstanceId, request.Message);
        await _jobStepInstanceLogRepository.AddAsync(jobStepInstanceLog);
        return Result.Success();
    }
}
