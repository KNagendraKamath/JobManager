
using JobScheduler.Domain.Abstractions;

namespace JobScheduler.Domain.Job;

public class JobStep:Entity
{
    public long JobId { get; set; }
    public long JobConfigId { get; set; }
    public string Parameter { get; set; }
}
