
using JobManager.Domain.Abstractions;

namespace JobManager.Domain.JobSchedulerInstance;

public class JobStepInstanceLog:Entity
{
    private JobStepInstanceLog() { }

    public JobStepInstanceLog(long jobStepInstanceId, string log)
    {
        JobStepInstanceId = jobStepInstanceId;
        Log = log;
        CreatedById = 1;
        CreatedTime = DateTime.UtcNow;
        Active = true;
    }

    public long JobStepInstanceId {  get; set; }
    public JobStepInstance JobStepInstance { get; set; }

    public string Log { get; set; }
}
