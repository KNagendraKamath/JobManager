using JobManager.Application.Abstractions.Messaging;

namespace JobManager.Application.JobSchedulerInstance.CreateJobInstance;
public record CreateJobStepInstanceCommand(long JobInstanceId, long JobStepId) : ICommand<long>;

