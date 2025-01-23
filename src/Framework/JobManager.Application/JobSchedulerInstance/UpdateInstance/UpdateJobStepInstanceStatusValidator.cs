using FluentValidation;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance.UpdateInstance;
internal sealed class UpdateJobStepInstanceStatusValidator : AbstractValidator<UpdateJobStepInstanceStatusCommand>
{
    public UpdateJobStepInstanceStatusValidator(IJobStepInstanceValidation jobStepInstanceValidation)
    {
        RuleFor(x => x.JobStepInstanceId)
               .GreaterThan(0)
               .MustAsync(async (jobStepInstanceId, cancellationToken) => await jobStepInstanceValidation.IsValidJobStepInstance(jobStepInstanceId, cancellationToken))
               .WithMessage(x => $"JobStepInstance with id {x.JobStepInstanceId} not found");

    }
}
