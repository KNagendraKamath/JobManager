
using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSetup;

namespace JobManager.Application.JobSetup.CreateJob;

internal class CreateJobCommandHandler : ICommandHandler<CreateJobCommand, long>
{
    private readonly IJobRepository _jobRepository;
    private readonly IJobConfigRepository _jobConfigRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateJobCommandHandler(IJobRepository jobRepository,
                                   IUnitOfWork unitOfWork,
                                   IJobConfigRepository jobConfigRepository)
    {
        _jobRepository = jobRepository;
        _unitOfWork = unitOfWork;
        _jobConfigRepository = jobConfigRepository;
    }

    public async Task<Result<long>> Handle(CreateJobCommand request, CancellationToken cancellationToken)
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

        _jobRepository.Add(job);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return job.Id;
    }
}
