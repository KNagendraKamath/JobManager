using JobManager.Framework.Domain.Abstractions;

namespace JobManager.Framework.Domain.JobSetup;

public class Job : Entity
{
    private Job()
    {
    }
    private Job(string? description,
                DateTime effectiveDateTime,
                string jobType)
    {
        EffectiveDateTime = effectiveDateTime;
        Description = description;
        Type = jobType;
    }

    public DateTime EffectiveDateTime { get; private set; }
    public string? Description { get; private set; }
    public string Type { get; private set; }
    public string CronExpression { get; private set; }
    public RecurringDetail? RecurringDetail { get; private set; }
    public List<JobStep> JobSteps { get; private set; } = new();

    public static Job Create(string? description,
                             DateTime effectiveDateTime,
                             string jobType) => 
        new Job(description, effectiveDateTime, jobType);

    public void SetRecurringDetail(RecurringDetail? recurringDetail)
    {
        RecurringDetail = recurringDetail;
    }

    public void AddJobStep(JobStep jobStep)
    {
        ArgumentNullException.ThrowIfNull(jobStep);
        JobSteps.Add(jobStep);
    }

    public void RemoveJobStep(JobStep jobStep)
    {
        ArgumentNullException.ThrowIfNull(jobStep);
        JobSteps.Remove(jobStep);
    }

    public void SetCronExpression(string cronExpression) => CronExpression = cronExpression;

}
