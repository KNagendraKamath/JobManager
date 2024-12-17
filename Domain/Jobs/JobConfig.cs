using JobScheduler.Domain.Abstractions;

namespace JobScheduler.Domain.Jobs;

public class JobConfig:Entity
{
    
    public string Name { get; set; }
}
