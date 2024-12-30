using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace JobManager.Infrastructure.JobSchedulerInstance.Scheduler;

public class JobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    public JobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        IJob? job =  _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
        if(job is null)
            throw new JobExecutionException($"Job of type {bundle.JobDetail.JobType} not found");
        return job;
    }

    public void ReturnJob(IJob job)
    {
        throw new NotImplementedException();
    }
}
