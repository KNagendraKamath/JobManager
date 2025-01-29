using JobManager.Framework.Application.Abstractions.Messaging;

namespace JobManager.Framework.Application.JobSchedulerInstance.UpdateInstance;
public record UpdateJobStepInstanceStatusCommand(long JobStepInstanceId, Status Status, DateTimeOffset Time) : ICommand;

