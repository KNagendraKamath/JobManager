
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSetup;

namespace JobManager.Domain.JobSchedulerInstance;

public class JobInstance:Entity
{

    private JobInstance() { }

    private JobInstance(long jobId,Status status)
    {
        JobId = jobId;
        Status = status;
        JobStepInstances = new();
        CreatedById = 1;
        CreatedTime = DateTime.UtcNow;
    }

    public long JobId {  get; private set; }
    public Job Job { get; private set; }
    public Status Status { get; private set; }
    public List<JobStepInstance> JobStepInstances { get; private set; }

    public static JobInstance Create(long jobId,Status status)
    {
         return new JobInstance(jobId,status);
    }

    public void AddJobStepInstance(JobStepInstance jobStepInstance)
    {
        if (jobStepInstance is null)
            throw new ArgumentNullException(nameof(JobStepInstance));
        JobStepInstances.Add(jobStepInstance);
    }

    public void RemoveJobStepInstance(JobStepInstance jobStepInstance)
    {
        if (jobStepInstance == null)
            throw new ArgumentNullException(nameof(jobStepInstance));
        JobStepInstances.Remove(jobStepInstance);
    }
}
