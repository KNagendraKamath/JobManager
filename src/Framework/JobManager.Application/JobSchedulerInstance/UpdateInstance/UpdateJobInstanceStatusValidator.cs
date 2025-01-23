using FluentValidation;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance.UpdateInstance;
internal sealed class UpdateJobInstanceStatusValidator : AbstractValidator<UpdateJobInstanceStatusCommand>
{
    public UpdateJobInstanceStatusValidator(IJobInstanceValidation jobInstanceValidation) => 
        RuleFor(x => x.JobInstanceId)
               .GreaterThan(0)
               .MustAsync(async (jobInstanceId, cancellationToken) => await jobInstanceValidation.IsValidJobInstance(jobInstanceId, cancellationToken))
               .WithMessage(x => $"JobInstance with id {x.JobInstanceId} not found");
}
