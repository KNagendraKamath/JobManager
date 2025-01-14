using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;
using JobManager.Domain.JobSetup;

namespace JobManager.Application.JobSetup.CreateJob;
internal sealed class UnscheduleCommandHandler : ICommandHandler<UnscheduleJobCommand>
{
    private readonly IJobRepository _jobRepository;
    private readonly IJobScheduler _jobScheduler;

    public UnscheduleCommandHandler(IJobRepository jobRepository, IJobScheduler jobScheduler)
    {
        _jobRepository = jobRepository;
        _jobScheduler = jobScheduler;
    }

    public async Task<Result> Handle(UnscheduleJobCommand request, CancellationToken cancellationToken)
    {
        Job job = await _jobRepository.GetByIdAsync(request.JobId, cancellationToken);

        if (job is null)
            return Result.Failure(Error.NotFound("NotFound", "Job not found"));

        IEnumerable<long> jobStepIds = GetJobStepIds(request.JobName, job);

        if (jobStepIds.Any()) return Result.Failure(Error.NotFound("NotFound", $"Job step {request.JobName} in Job with Id: {request.JobId} not found"));

        DeactivateJobSteps(job, jobStepIds);

        await _jobScheduler.UnSchedule(request.JobId, jobStepIds);

        return Result.Success();
    }

    private IEnumerable<long> GetJobStepIds(string? JobName, Job job)
    {
        if (JobName is not null)
        {
            JobStep? stepToDeactivate = job.JobSteps.FirstOrDefault(x => x.JobConfig.Name == JobName);
            return stepToDeactivate is not null ? [stepToDeactivate.Id] : [];
        }
        return job.JobSteps.Select(x => x.Id);
    }

    private void DeactivateJobSteps(Job job, IEnumerable<long> jobStepIds) => 
        job.JobSteps.Where(x => jobStepIds.Contains(x.Id)).ToList().ForEach(x => x.Deactivate());
}
