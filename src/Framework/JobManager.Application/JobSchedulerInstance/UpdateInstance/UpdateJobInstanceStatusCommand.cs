using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Application.JobSchedulerInstance.UpdateInstance;
public record UpdateJobInstanceStatusCommand(long JobInstanceId, Status Status) : ICommand;

