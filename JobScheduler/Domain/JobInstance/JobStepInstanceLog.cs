
using JobScheduler.Domain.Abstractions;

namespace JobScheduler.Domain.JobInstance;

public class JobStepInstanceLog:Entity
{
    public long JobStepInstanceId {  get; set; }
    public string Log { get; set; }
}
