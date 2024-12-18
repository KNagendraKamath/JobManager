
using JobScheduler.Domain.Abstractions;

namespace JobScheduler.Domain.Models;

public class JobStepInstanceLog:Entity
{
    public long JobStepInstanceId {  get; set; }
    public string Log { get; set; }
}
