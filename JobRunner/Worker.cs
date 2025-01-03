using JobManager.Application.JobSetup.GetJobDetail;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSetup;
using JobManager.Infrastructure.JobSchedulerInstance.Scheduler;
using MediatR;
using Quartz;
using Quartz.Impl.Matchers;

namespace JobRunner;

public class Worker : QuartzScheduler
{
    public Worker(ISchedulerFactory schedulerFactory, ISender sender, ILogger<QuartzScheduler> logger) : base(schedulerFactory, sender, logger)
    {
    }
}
