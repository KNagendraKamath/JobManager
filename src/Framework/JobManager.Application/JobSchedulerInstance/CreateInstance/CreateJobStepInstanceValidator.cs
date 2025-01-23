using FluentValidation;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance.CreateInstance;

internal sealed class CreateJobStepInstanceValidator:AbstractValidator<CreateJobStepInstanceCommand>
{
    public CreateJobStepInstanceValidator(IJobInstanceValidation jobInstanceValidation) => 
        RuleFor(x => x.JobInstanceId)
               .GreaterThan(0)
               .MustAsync(async (jobInstanceId, cancellationToken) => await jobInstanceValidation.IsValidJobInstance(jobInstanceId, cancellationToken))
               .WithMessage(x => $"JobInstance with id {x.JobInstanceId} not found");
}
