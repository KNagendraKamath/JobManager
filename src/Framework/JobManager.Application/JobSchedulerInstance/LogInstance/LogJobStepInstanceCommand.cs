using JobManager.Framework.Application.Abstractions.Messaging;

namespace JobManager.Framework.Application.JobSchedulerInstance.LogInstance;
public record LogJobStepInstanceCommand(long JobStepInstanceId, string Message) : ICommand;
