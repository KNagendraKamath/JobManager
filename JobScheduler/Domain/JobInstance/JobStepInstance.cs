
using JobScheduler.Domain.Abstractions;

namespace JobScheduler.Domain.Models;

public class JobStepInstance:Entity
{
    public long JobInstanceId {  get; set; }
    public Status JobStatus {  get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public List<JobStepInstanceLog> JobStepInstanceLogs { get; set; }
}
