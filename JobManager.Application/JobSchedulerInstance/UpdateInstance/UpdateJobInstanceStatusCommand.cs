using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.JobSchedulerInstance;

namespace JobManager.Application.JobSchedulerInstance.UpdateJobInstance;
public record UpdateJobInstanceStatusCommand(long JobInstanceId, Status Status) : ICommand;

