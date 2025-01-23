using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance.UpdateInstance;
public record UpdateJobStepInstanceStatusCommand(long JobStepInstanceId, Status Status, DateTimeOffset Time) : ICommand;

