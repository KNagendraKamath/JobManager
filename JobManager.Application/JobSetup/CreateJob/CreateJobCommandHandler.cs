
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
        Job job = Job.Create(request.Description,
                             request.EffectiveDateTime,
                             request.JobType,
                             request.RecurringType,
                             CronExpressionBuilder(request.EffectiveDateTime, request.JobType, request.RecurringType),
                             request.CreatedById);

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

    private string CronExpressionBuilder(DateTime effectiveDateTime, JobType jobType, RecurringType recurringType)
    {
        if (jobType == JobType.Onetime)
            return $"{effectiveDateTime.Minute} {effectiveDateTime.Hour} {effectiveDateTime.Day} {effectiveDateTime.Month} ? {effectiveDateTime.Year}";
        
        //For recurring jobs, generate the appropriate cron expression based on the recurring type
        return recurringType switch
        {
            RecurringType.EveryMinute => "0 * * * * ?",
            RecurringType.EverySecond => "* * * * * ?",
            RecurringType.Daily => $"{effectiveDateTime.Minute} {effectiveDateTime.Hour} * * ?",
            RecurringType.Weekly => $"{effectiveDateTime.Minute} {effectiveDateTime.Hour} ? * {effectiveDateTime.DayOfWeek.ToString().Substring(0, 3).ToUpper()}",
            RecurringType.Monthly => $"{effectiveDateTime.Minute} {effectiveDateTime.Hour} {effectiveDateTime.Day} * ?",
            _ => throw new NotImplementedException($"Recurring type {recurringType} is not supported")
        };
    }
}
