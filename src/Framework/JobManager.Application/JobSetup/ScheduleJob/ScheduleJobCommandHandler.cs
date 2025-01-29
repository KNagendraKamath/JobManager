using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Application.JobSetup.ScheduleJob;

internal sealed class ScheduleJobCommandHandler : ICommandHandler<ScheduleJobCommand, long>
{
    private readonly IJobRepository _jobRepository;
    private readonly IJobConfigRepository _jobConfigRepository;
    private readonly ICronExpressionGenerator _cronExpressionGenerator;

    public ScheduleJobCommandHandler(IJobRepository jobRepository,
                                   IJobConfigRepository jobConfigRepository,
                                   ICronExpressionGenerator cronExpressionGenerator)
    {
        _jobRepository = jobRepository;
        _jobConfigRepository = jobConfigRepository;
        _cronExpressionGenerator = cronExpressionGenerator;
    }

    public async Task<Result<long>> Handle(ScheduleJobCommand request, CancellationToken cancellationToken)
    {
        Job job = Job.Create(request.Description,
                             request.EffectiveDateTime,
                             request.JobType);

        if (request.JobType == JobType.Recurring && request.RecurringDetail is not null)
        {
            job.SetRecurringDetail(new Domain.JobSetup.RecurringDetail(job,
                                                                      request.RecurringDetail.RecurringType,
                                                                      request.RecurringDetail.Second,
                                                                      request.RecurringDetail.Minute,
                                                                      request.RecurringDetail.Hour,
                                                                      request.RecurringDetail.DayOfWeek,
                                                                      request.RecurringDetail.Day));

            job.SetCronExpression(_cronExpressionGenerator.Generate(request.RecurringDetail.RecurringType,
                                                                      request.RecurringDetail.Second,
                                                                      request.RecurringDetail.Minute,
                                                                      request.RecurringDetail.Hour,
                                                                      request.RecurringDetail.Day,
                                                                      request.RecurringDetail.DayOfWeek
                                                                     ));


        }

        IEnumerable<JobConfig> jobConfigs = await _jobConfigRepository.GetJobConfigByNamesAsync(request.JobSteps.Select(x => x.JobName).ToArray(), cancellationToken);
        request.JobSteps.ForEach(step =>
           job.AddJobStep(new JobStep(job,
                                      jobConfigs.First(x => x.Name.Equals(step.JobName, StringComparison.Ordinal)),
                                      step.JsonParameter))
       );

        await _jobRepository.AddAsync(job);

        return job.Id;
    }


}
