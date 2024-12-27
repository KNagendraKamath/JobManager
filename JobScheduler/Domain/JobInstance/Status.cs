namespace JobScheduler.Domain.JobInstance;

public enum Status
{
    NotStarted,
    Running,
    Completed,
    CompletedWithErrors,
    Faulted
}
