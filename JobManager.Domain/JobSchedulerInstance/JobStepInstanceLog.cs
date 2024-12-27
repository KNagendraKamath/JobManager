
using JobManager.Domain.Abstractions;

namespace JobManager.Domain.JobSchedulerInstance;

public class JobStepInstanceLog:Entity
{
    private JobStepInstanceLog() { }

    public long JobStepInstanceId {  get; set; }
    public string Log { get; set; }
}
