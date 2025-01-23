using FluentValidation;
using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Application.JobSetup.CreateJob;
internal sealed class UnscheduleJobValidator:AbstractValidator<UnscheduleJobCommand>
{
    public UnscheduleJobValidator(IJobValidation jobValidation,
                                  IJobStepValidation jobStepValidation)
    {
        RuleFor(x => x.JobId).GreaterThan(0)
                             .MustAsync(async (jobId,cancellationToken) =>  await jobValidation.IsValidJob(jobId,cancellationToken))
                             .WithMessage(x=>$"Job with {x.JobId} not found");

        RuleFor(x => x).MustAsync(async (request,cancellationToken) => await jobStepValidation.IsValidJobStep(request.JobId,request.JobName!,cancellationToken))
                       .When(x => x.JobName is not null)
                       .WithMessage(x => $"Job step {x.JobName} in Job with Id: {x.JobId} not found");
    }
}
