using System.Collections.ObjectModel;
using JobManager.Domain.Abstractions;

namespace JobManager.Domain.JobSetup;

public class Job : Entity
{
    private Job()
    {
    }
    private Job(string? description,
                DateTime effectiveDateTime,
                JobType jobType,
                RecurringType recurringType,
                long createdById)
    {
        EffectiveDateTime = effectiveDateTime;
        Description = description;
        Type = jobType;
        RecurringType = recurringType;
        Active = true;
        CreatedById = createdById;
        JobSteps = new();
    }

    public DateTime EffectiveDateTime { get; private set; }
    public string? Description { get; private set; }
    public JobType Type { get; private set; }
    public RecurringType RecurringType { get; private set; }
    public List<JobStep> JobSteps { get; private set; }

    public static Job Create(string? description,
                             DateTime effectiveDateTime,
                             JobType jobType,
                             RecurringType recurringType,
                             long createdById)
    {
        return new Job(description, effectiveDateTime, jobType, recurringType, createdById);
    }

    public void AddJobStep(JobStep jobStep)
    {
        if (jobStep is null)
            throw new ArgumentNullException(nameof(JobStep));

        JobSteps.Add(jobStep);
    }

    public void RemoveJobStep(JobStep jobStep)
    {
        if (jobStep == null)
            throw new ArgumentNullException(nameof(jobStep));

        JobSteps.Remove(jobStep);
    }
}

public enum JobType
{
    None,
    Onetime,
    Recurring
}

public enum RecurringType
{
    None,
    EveryMinute,
    EverySecond,
    Daily,
    Weekly,
    Monthly
}
