

using JobManager.Framework.Application.JobSetup.UnscheduleJob;
using Microsoft.Extensions.DependencyInjection;

namespace JobManager.Framework.Infrastructure.JobSchedulerInstance.Scheduler;
public interface IJobScheduler
{
    Task UnSchedule(IReadOnlyList<JobGroups> jobGroups);

    Task ExecuteAsync(IServiceScope scope,CancellationToken cancellationToken = default);
}
