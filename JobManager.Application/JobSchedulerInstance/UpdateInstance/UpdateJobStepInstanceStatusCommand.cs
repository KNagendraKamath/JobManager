using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.JobSchedulerInstance;

namespace JobManager.Application.JobSchedulerInstance.UpdateJobInstance;
public record UpdateJobStepInstanceStatusCommand(long JobStepInstanceId, Status Status, DateTimeOffset Time) : ICommand;

