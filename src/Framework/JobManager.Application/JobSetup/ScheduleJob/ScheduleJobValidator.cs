using FluentValidation;
using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Application.JobSetup.ScheduleJob;
internal sealed class ScheduleJobValidator : AbstractValidator<ScheduleJobCommand>
{
    private const int EffectiveDateTimeOffset = -10;

    public ScheduleJobValidator(IJobConfigValidation jobConfigValidation)
    {
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.EffectiveDateTime).GreaterThanOrEqualTo(DateTime.UtcNow.AddSeconds(EffectiveDateTimeOffset));
        RuleFor(x => x.JobType).NotEqual(JobType.None)
                               .WithMessage("Job Type must be specified");

        RuleFor(x => x.JobSteps)
            .NotEmpty();

        RuleForEach(x => x.JobSteps)
            .MustAsync(async (step, cancellationToken) => await jobConfigValidation.IsValidJobConfig(step.JobName, cancellationToken));

        RuleFor(x => x.RecurringDetail)
           .NotEmpty()
           .When(x => x.JobType == JobType.Recurring)
           .WithMessage("Recurring Detail must be specified if JobType is Recurring")
           .SetValidator(new RecurringDetailValidator())
           .When(x => x.JobType == JobType.Recurring);
    }
}

internal sealed class JobStepValidator : AbstractValidator<Step>
{
    public JobStepValidator(IJobConfigValidation jobConfigValidation)
    {
        RuleFor(x => x.JobName)
               .NotEmpty()
               .MustAsync(async (jobName, cancellationToken) => await jobConfigValidation.IsValidJobConfig(jobName, cancellationToken))
               .WithMessage(x => $"{x.JobName} is invalid Job");
    }
}

internal sealed class RecurringDetailValidator : AbstractValidator<RecurringDetail?>
{
    private const int MaxTimeUnit = 59;
    private const int MaxHourLimit = 23;
    private const int MaxDayLimit = 31;
    private const int MinDayLimit = 1;

    public RecurringDetailValidator()
    {
        RuleFor(recurringDetail => recurringDetail!.Second).InclusiveBetween(0, MaxTimeUnit);
        RuleFor(recurringDetail => recurringDetail!.Minute).InclusiveBetween(0, MaxTimeUnit);
        RuleFor(recurringDetail => recurringDetail!.Hour).InclusiveBetween(0, MaxHourLimit);
        RuleFor(recurringDetail => recurringDetail!.Day).InclusiveBetween(MinDayLimit, MaxDayLimit);
    }
}
