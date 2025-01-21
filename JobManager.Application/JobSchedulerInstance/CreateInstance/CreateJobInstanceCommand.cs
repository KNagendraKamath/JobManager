using JobManager.Framework.Application.Abstractions.Messaging;

namespace JobManager.Framework.Application.JobSchedulerInstance.CreateInstance;
public record CreateJobInstanceCommand(long JobId) : ICommand<long>;

