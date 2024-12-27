using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.JobSetup;

namespace JobManager.Application.JobSetup.CreateJob;

public record CreateJobCommand(
    string? Description,
    DateTime EffectiveDateTime,
    JobType JobType,
    RecurringType RecurringType,
    List<Step> JobSteps,
    long CreatedById) : ICommand<long>;

public record Step(string JobName,string JsonParameter);


