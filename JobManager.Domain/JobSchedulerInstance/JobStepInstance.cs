
using JobManager.Domain.Abstractions;

namespace JobManager.Domain.JobSchedulerInstance;

public class JobStepInstance:Entity
{
    private JobStepInstance() { }

    public long JobInstanceId {  get; set; }
    public Status JobStatus {  get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public List<JobStepInstanceLog> JobStepInstanceLogs { get; set; }
}
