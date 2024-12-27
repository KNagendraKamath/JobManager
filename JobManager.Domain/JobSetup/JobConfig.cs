using JobManager.Domain.Abstractions;

namespace JobManager.Domain.JobSetup;

public class JobConfig:Entity
{
    private JobConfig() { }
    public string Name { get; init; }
}
