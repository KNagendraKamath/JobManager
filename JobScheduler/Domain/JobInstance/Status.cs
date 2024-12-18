namespace JobScheduler.Domain.Models;

public enum Status
{
    NotStarted,
    Running,
    Completed,
    CompletedWithErrors,
    Faulted
}
