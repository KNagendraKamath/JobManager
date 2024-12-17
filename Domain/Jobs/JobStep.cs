
using JobScheduler.Domain.Abstractions;

namespace JobScheduler.Domain.Jobs;

public class JobStep:Entity
{
    public long JobId { get; set; }
    public long JobConfigId { get; set; }
    public string JobParameter { get; set; }

}
