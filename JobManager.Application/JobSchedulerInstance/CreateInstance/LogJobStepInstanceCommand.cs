using JobManager.Application.Abstractions.Messaging;

namespace JobManager.Application.JobSchedulerInstance.CreateJobInstance;
public record LogJobStepInstanceCommand(long JobStepInstanceId,string Message):ICommand;
