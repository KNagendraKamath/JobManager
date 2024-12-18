
using JobScheduler.Domain.Abstractions;

namespace JobScheduler.Domain.Jobs;

public class Jobs:Entity
{
    private Job(string? description,
               DateTime effectiveDateTime,
               JobType jobType,
               RecurringType? recurringType,
               List<JobStep> jobSteps)
    {
        EffectiveDateTime = effectiveDateTime;
        Description = description;
        Type = jobType;
        RecurringType = recurringType;
        JobSteps = jobSteps;
    }

    public DateTime EffectiveDateTime { get; set; }
    public string? Description { get; set; }
    public JobType Type { get; set; }
    public RecurringType? RecurringType { get; set; }
    public List<JobStep> JobSteps { get; set; } = new();

    public static Job Create(string? description,
                             DateTime effectiveDateTime,
                             JobType jobType,
                             RecurringType? recurringType,
                             List<JobStep> jobSteps)
    {
        return new Job(description, effectiveDateTime, jobType, recurringType, jobSteps);
    }


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
