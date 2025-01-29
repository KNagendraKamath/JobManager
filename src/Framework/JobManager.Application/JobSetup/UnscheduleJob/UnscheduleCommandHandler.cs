﻿using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Application.JobSetup.CreateJob;
internal sealed class UnscheduleCommandHandler : ICommandHandler<UnscheduleJobCommand>
{
    private readonly IJobRepository _jobRepository;

    public UnscheduleCommandHandler(IJobRepository jobRepository) => 
        _jobRepository = jobRepository;

    public async Task<Result> Handle(UnscheduleJobCommand request, CancellationToken cancellationToken)
    {
        Job job = await _jobRepository.GetByIdAsync(request.JobId, cancellationToken);

        if (job is null)
            return Result.Failure(Error.NotFound("NotFound", "Job not found"));

        List<long> jobStepIds = GetJobStepIds(request.JobName, job);

        if (jobStepIds.Any()) return Result.Failure(Error.NotFound("NotFound", $"Job step {request.JobName} in Job with Id: {request.JobId} not found"));

        DeactivateJobSteps(job, jobStepIds);

        return Result.Success();
    }

    private List<long> GetJobStepIds(string? JobName, Job job)
    {
        if (JobName is not null)
        {
            JobStep? stepToDeactivate = job.JobSteps.Find(x => x.JobConfig.Name == JobName);
            return stepToDeactivate is not null ? [stepToDeactivate.Id] : [];
        }
        return job.JobSteps.Select(x => x.Id).ToList();
    }

    private void DeactivateJobSteps(Job job, IEnumerable<long> jobStepIds) => 
        job.JobSteps.Where(x => jobStepIds.Contains(x.Id)).ToList().ForEach(x => x.Deactivate());
}
