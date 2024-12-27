using JobManager.Application.Abstractions.Messaging;

namespace JobManager.Application.JobSchedulerInstance.CreateJobInstance;
public record CreateJobInstanceCommand(List<long> JobIds) : ICommand<long>;

