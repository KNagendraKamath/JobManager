
using JobManager.Framework.Domain.Abstractions;

namespace JobManager.Framework.Domain.JobSchedulerInstance;

public class JobStepInstanceLog:Entity
{
    private JobStepInstanceLog() { }

    public JobStepInstanceLog(long jobStepInstanceId, string log)
    {
        JobStepInstanceId = jobStepInstanceId;
        Log = log;
    }

    public long JobStepInstanceId {  get; set; }
    public JobStepInstance JobStepInstance { get; set; }

    public string Log { get; set; }
}
