using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Domain.JobSchedulerInstance;

public class JobInstance:Entity
{
    private JobInstance() { }

    private JobInstance(long jobId,string status)
    {
        JobId = jobId;
        Status = status;
        JobStepInstances = new();
    }

    public long JobId {  get; private set; }
    public Job Job { get; private set; }
    public string Status { get; private set; }
    public List<JobStepInstance> JobStepInstances { get; private set; }=new();

    public static JobInstance Create(long jobId, string status) => new JobInstance(jobId, status);

    public void AddJobStepInstance(JobStepInstance jobStepInstance)
    {
        ArgumentNullException.ThrowIfNull(jobStepInstance);
        JobStepInstances.Add(jobStepInstance);
    }

    public void RemoveJobStepInstance(JobStepInstance jobStepInstance)
    {
        ArgumentNullException.ThrowIfNull(jobStepInstance);
        JobStepInstances.Remove(jobStepInstance);
    }

    public void UpdateStatus(string status) => Status = status;
}
