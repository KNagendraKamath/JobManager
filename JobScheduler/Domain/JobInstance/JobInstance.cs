
using JobScheduler.Domain.Abstractions;

namespace JobScheduler.Domain.Models;

public class JobInstance:Entity
{
    public long JobId {  get; set; }
    public Status Status { get; set; }
    public List<JobStepInstance> JobStepInstance { get; set; }
}
