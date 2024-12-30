using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.JobSchedulerInstance;

namespace JobManager.Application.JobSchedulerInstance.CreateJobInstance;
public record UpdateJobStepInstanceStatusCommand(long JobStepInstanceId, Status Status,DateTimeOffset Time) : ICommand;

