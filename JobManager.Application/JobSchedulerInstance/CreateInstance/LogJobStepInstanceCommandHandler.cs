using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;

namespace JobManager.Application.JobSchedulerInstance.CreateJobInstance;
internal class LogJobStepInstanceCommandHandler : ICommandHandler<LogJobStepInstanceCommand>
{
    private readonly IJobStepInstanceRepository _jobStepInstanceRepository;
    private readonly IUnitOfWork _unitofWork;


    public LogJobStepInstanceCommandHandler(IJobStepInstanceRepository jobStepInstanceRepository, IUnitOfWork unitofWork)
    {
        _jobStepInstanceRepository = jobStepInstanceRepository;
        _unitofWork = unitofWork;
    }


    public async Task<Result> Handle(LogJobStepInstanceCommand request, CancellationToken cancellationToken)
    {
        JobStepInstance? jobStepInstance = await _jobStepInstanceRepository.GetByIdAsync(request.JobStepInstanceId, cancellationToken);

        if (jobStepInstance is null)
            throw new InvalidOperationException($"JobStepInstance with Id {request.JobStepInstanceId} not found");

        jobStepInstance.AddLog(request.Message);
        _jobStepInstanceRepository.Update(jobStepInstance);
        await _unitofWork.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }
}
