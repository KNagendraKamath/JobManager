
using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSetup;

namespace JobManager.Application.JobSetup.CreateJob;

internal class ScheduleJobCommandHandler : ICommandHandler<ScheduleJobCommand, long>
{
    private readonly IJobRepository _jobRepository;
    private readonly IJobConfigRepository _jobConfigRepository;

    public ScheduleJobCommandHandler(IJobRepository jobRepository,
                                   IJobConfigRepository jobConfigRepository)
    {
        _jobRepository = jobRepository;
        _jobConfigRepository = jobConfigRepository;
    }

    public async Task<Result<long>> Handle(ScheduleJobCommand request, CancellationToken cancellationToken)
    {
        DateTime effectiveDateTime = request.EffectiveDateTime.AddMinutes(1);
        Job job = Job.Create(request.Description,
                             effectiveDateTime,
                             request.JobType);

        if (request.JobType == JobType.Recurring && request.RecurringDetail is not null)
            job.SetRecurringDetail(new Domain.JobSetup.RecurringDetail(job,
                                                                      request.RecurringDetail.RecurringType,
                                                                      request.RecurringDetail.Second,
                                                                      request.RecurringDetail.Minutes,
                                                                      request.RecurringDetail.Hours,
                                                                      request.RecurringDetail.DayOfWeek,
                                                                      request.RecurringDetail.Day));

        foreach (Step step in request.JobSteps)
        {
            JobConfig jobConfig = await _jobConfigRepository.GetJobConfigAsync(step.JobName, cancellationToken);
            if (jobConfig == null)
                return Result.Failure<long>(Error.NotFound(nameof(JobStep), $"{step.JobName} is invalid Job"));

            job.AddJobStep(new JobStep(job,
                                       jobConfig,
                                       step.JsonParameter));
        }

        await _jobRepository.AddAsync(job);

        return job.Id;
    }
}
