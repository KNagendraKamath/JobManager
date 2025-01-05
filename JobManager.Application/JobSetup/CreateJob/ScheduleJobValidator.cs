using FluentValidation;
using JobManager.Domain.JobSetup;

namespace JobManager.Application.JobSetup.CreateJob;
internal class ScheduleJobValidator : AbstractValidator<ScheduleJobCommand>
{
    private const int EffectiveDateTimeOffset = -10;

    public ScheduleJobValidator()
    {
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.EffectiveDateTime).GreaterThanOrEqualTo(DateTime.UtcNow.AddSeconds(EffectiveDateTimeOffset));
        RuleFor(x => x.JobType).NotEqual(JobType.None)
                               .WithMessage("Job Type must be specified");

        RuleFor(x => x.JobSteps).NotEmpty();

        RuleFor(x => x.RecurringDetail)
           .NotEmpty()
           .When(x => x.JobType == JobType.Recurring)
           .WithMessage("Recurring Detail must be specified if JobType is Recurring")
           .SetValidator(new RecurringDetailValidator())
           .When(x => x.JobType == JobType.Recurring);

    }
}

internal class RecurringDetailValidator : AbstractValidator<RecurringDetail?>
{
    private const int MaxTimeUnit = 59;
    private const int MaxHourLimit = 23;
    private const int MaxDayLimit = 31;
    private const int MinDayLimit = 1;

    public RecurringDetailValidator()
    {
        RuleFor(recurringDetail => recurringDetail!.Second).InclusiveBetween(0, MaxTimeUnit);
        RuleFor(recurringDetail => recurringDetail!.Minutes).InclusiveBetween(0, MaxTimeUnit);
        RuleFor(recurringDetail => recurringDetail!.Hours).InclusiveBetween(0, MaxHourLimit);
        RuleFor(recurringDetail => recurringDetail!.Day).InclusiveBetween(MinDayLimit, MaxDayLimit);
    }
}
