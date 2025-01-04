using FluentValidation;
using JobManager.Domain.JobSetup;
using MediatR;

namespace JobManager.Application.JobSetup.CreateJob;
internal class UnscheduleJobValidator:AbstractValidator<UnscheduleJobCommand>
{
    private readonly IJobRepository _jobRepository;

    public UnscheduleJobValidator(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;

        RuleFor(x => x.JobId).GreaterThan(0)
                             .MustAsync(BeValidJob)
                             .WithMessage(x=>$"Job with {x.JobId} not found");

        RuleFor(x => x).MustAsync(BeNullOrValidJobStep)
                       .When(x => x.JobName is not null)
                       .WithMessage(x => $"Job step {x.JobName} in Job with Id: {x.JobId} not found");
    }

    private async Task<bool> BeNullOrValidJobStep(UnscheduleJobCommand command, CancellationToken token)
    {
        Job job = await _jobRepository.GetByIdAsync(command.JobId, token);
        JobStep step = job!.JobSteps.FirstOrDefault(x => x.JobConfig.Name == command.JobName);
        return step is not null;
    }

    private async Task<bool> BeValidJob(long jobId, CancellationToken token)
    {
       Job job =  await _jobRepository.GetByIdAsync(jobId, token);
       return job is not null;
    }
}
