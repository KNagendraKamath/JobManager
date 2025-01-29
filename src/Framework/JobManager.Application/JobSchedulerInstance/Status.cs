namespace JobManager.Framework.Application.JobSchedulerInstance;

public enum Status
{
    Unknown,
    NotStarted,
    Running,
    Completed,
    CompletedWithErrors,
    Faulted
}
