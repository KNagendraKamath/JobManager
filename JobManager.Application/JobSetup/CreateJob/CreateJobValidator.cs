using FluentValidation;
using JobManager.Domain.JobSetup;

namespace JobManager.Application.JobSetup.CreateJob;
internal class CreateJobValidator:AbstractValidator<CreateJobCommand>
{
    public CreateJobValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.EffectiveDateTime).GreaterThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.JobType).NotEqual(JobType.None).WithMessage("Job Type must be specified");
        RuleFor(x => x.JobSteps).NotEmpty();
    }
}
