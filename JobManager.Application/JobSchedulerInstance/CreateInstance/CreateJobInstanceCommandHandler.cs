using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;
using MediatR;

namespace JobManager.Application.JobSchedulerInstance.CreateJobInstance;
internal sealed class CreateJobInstanceCommandHandler : ICommandHandler<CreateJobInstanceCommand,long>
{
    private readonly IJobInstanceRepository _jobInstanceRepository;

    public CreateJobInstanceCommandHandler(IJobInstanceRepository jobInstanceRepository)
    {
        _jobInstanceRepository = jobInstanceRepository;
    }

    async Task<Result<long>> IRequestHandler<CreateJobInstanceCommand, Result<long>>.Handle(CreateJobInstanceCommand request, CancellationToken cancellationToken)
    {
        JobInstance jobInstance = JobInstance.Create(request.JobId,Status.NotStarted);
        await _jobInstanceRepository.AddAsync(jobInstance);
        return jobInstance.Id;
    }
}
