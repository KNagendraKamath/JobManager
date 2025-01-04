using FluentValidation;
using JobManager.Domain.JobSetup;

namespace JobManager.Application.JobSetup.CreateJob;
internal class CreateJobValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.EffectiveDateTime).GreaterThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.JobType).NotEqual(JobType.None)
                               .WithMessage("Job Type must be specified");

        RuleFor(x => x.JobSteps).NotEmpty();

        RuleFor(x => x.RecurringDetail).SetValidator(new RecurringDetailValidator())
                                       .When(x => x.JobType == JobType.Recurring);
    }
}

internal class RecurringDetailValidator : AbstractValidator<RecurringDetail?>
{
    public RecurringDetailValidator()
    {
        RuleFor(recurringDetail => recurringDetail!.Second).InclusiveBetween(0, 59);
        RuleFor(recurringDetail => recurringDetail!.Minutes).InclusiveBetween(0, 59);
        RuleFor(recurringDetail => recurringDetail!.Hours).InclusiveBetween(0, 23);
        RuleFor(recurringDetail => recurringDetail!.Day).InclusiveBetween(1, 31);
    }
}
