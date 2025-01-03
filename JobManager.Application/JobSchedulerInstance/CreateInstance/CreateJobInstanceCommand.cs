using JobManager.Application.Abstractions.Messaging;

namespace JobManager.Application.JobSchedulerInstance.CreateJobInstance;
public record CreateJobInstanceCommand(long JobId) : ICommand<long>;

