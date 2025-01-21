using JobManager.Framework.Domain.Abstractions;

namespace JobManager.Framework.Domain.JobSetup;

public class JobConfig:Entity
{
    private JobConfig() { }
    public string Name { get; init; }
    public string Assembly { get; init; }
}
