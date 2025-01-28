using System.Reflection;

namespace JobManager.Framework.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;

public static class BaseJobInstanceAssemblyReference
{
    public static readonly Type Assembly = typeof(BaseJobInstance<>);
}
