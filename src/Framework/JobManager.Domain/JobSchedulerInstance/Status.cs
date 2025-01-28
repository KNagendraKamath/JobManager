namespace JobManager.Framework.Domain.JobSchedulerInstance;

public enum Status
{
    Unknown,
    NotStarted,
    Running,
    Completed,
    CompletedWithErrors,
    Faulted
}
