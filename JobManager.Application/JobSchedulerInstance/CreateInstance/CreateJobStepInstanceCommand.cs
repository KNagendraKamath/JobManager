using JobManager.Framework.Application.Abstractions.Messaging;

namespace JobManager.Framework.Application.JobSchedulerInstance.CreateInstance;
public record CreateJobStepInstanceCommand(long JobInstanceId, long JobStepId) : ICommand<long>;

