using JobScheduler.Domain.Abstractions;

namespace JobScheduler.Domain.Job;

public class JobConfig:Entity
{
    public string Name { get; set; }
}
