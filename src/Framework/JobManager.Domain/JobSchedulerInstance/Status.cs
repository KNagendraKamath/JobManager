namespace JobManager.Framework.Domain.JobSchedulerInstance;

public enum Status
{
    NotStarted,
    Running,
    Completed,
    CompletedWithErrors,
    Faulted
}
