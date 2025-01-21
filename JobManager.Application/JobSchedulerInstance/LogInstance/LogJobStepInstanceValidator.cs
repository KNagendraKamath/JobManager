using FluentValidation;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance.LogInstance;

internal class LogJobStepInstanceValidator : AbstractValidator<LogJobStepInstanceCommand>
{
    internal LogJobStepInstanceValidator(IJobStepInstanceValidation jobStepInstanceValidation)
    {
        RuleFor(x => x.JobStepInstanceId)
               .GreaterThan(0)
               .MustAsync(async (jobStepInstanceId, cancellationToken) => await jobStepInstanceValidation.IsValidJobStepInstance(jobStepInstanceId, cancellationToken))
               .WithMessage(x => $"JobStepInstance with id {x.JobStepInstanceId} not found");
    }
}
