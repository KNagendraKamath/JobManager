
using JobScheduler.Domain.Abstractions;

namespace JobScheduler.Domain.JobInstance;

public class JobInstance:Entity
{
    public long JobId {  get; set; }
    public Status Status { get; set; }
    public List<JobStepInstance> JobStepInstance { get; set; }
}
