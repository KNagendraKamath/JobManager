using JobManager.Domain.Abstractions;

namespace JobManager.Domain.JobSetup;

public class Job : Entity
{
    private Job()
    {
    }
    private Job(string? description,
                DateTime effectiveDateTime,
                JobType jobType
                )
    {
        EffectiveDateTime = effectiveDateTime;
        Description = description;
        Type = jobType;
        JobSteps = new();
    }

    public DateTime EffectiveDateTime { get; private set; }
    public string? Description { get; private set; }
    public JobType Type { get; private set; }
    public RecurringDetail? RecurringDetail { get; private set; }
    public List<JobStep> JobSteps { get; private set; }

    public static Job Create(string? description,
                             DateTime effectiveDateTime,
                             JobType jobType)
    {
        return new Job(description, effectiveDateTime, jobType);
    }

    public void SetRecurringDetail(RecurringDetail recurringDetail)
    {
        RecurringDetail = recurringDetail;
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
