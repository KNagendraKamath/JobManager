using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;
using MediatR;

namespace JobManager.Application.JobSchedulerInstance.CreateJobInstance;
internal sealed class CreateJobInstanceCommandHandler : ICommandHandler<CreateJobInstanceCommand,long>
{
    private readonly IJobInstanceRepository _jobInstanceRepository;
    private readonly IUnitOfWork _unitofWork;

    public CreateJobInstanceCommandHandler(IJobInstanceRepository jobInstanceRepository, IUnitOfWork unitofWork)
    {
        _jobInstanceRepository = jobInstanceRepository;
        _unitofWork = unitofWork;
    }

    async Task<Result<long>> IRequestHandler<CreateJobInstanceCommand, Result<long>>.Handle(CreateJobInstanceCommand request, CancellationToken cancellationToken)
    {
        JobInstance jobInstance = JobInstance.Create(request.JobId,Status.NotStarted);
         _jobInstanceRepository.Add(jobInstance);
        await _unitofWork.SaveChangesAsync(cancellationToken);
        return jobInstance.Id;
    }
}
