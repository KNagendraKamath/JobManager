using JobManager.Framework.Application.Abstractions.Messaging;

namespace JobManager.Framework.Application.JobSchedulerInstance.UpdateInstance;
public record UpdateJobInstanceStatusCommand(long JobInstanceId, Status Status) : ICommand;

