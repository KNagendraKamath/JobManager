
using JobScheduler.Domain.Abstractions;

namespace JobScheduler.Domain.Jobs;

public class Job:Entity
{
    public DateTime EffectiveDateTime { get; set; }
    public string? JobDescription { get; set; }
    public JobType JobType { get; set; }
    public RecurringType? RecurringType { get; set; }
    public List<JobStep> JobSteps { get; set; } = new();
}
public enum JobType
{
    Onetime,
    Recurring
}
public enum RecurringType
{
    EveryMinute,
    EverySecond,
    Daily,
    Weekly,
    Monthly
}
