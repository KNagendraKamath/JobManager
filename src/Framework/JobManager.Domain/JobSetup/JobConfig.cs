using JobManager.Framework.Domain.Abstractions;

namespace JobManager.Framework.Domain.JobSetup;

public class JobConfig:Entity
{
    public JobConfig() { }

    private JobConfig(string name)
    {
        Name = name;
        Active = true;
        CreatedTime = DateTime.UtcNow;
    }

    public string Name { get; private set; }

    public static JobConfig Create(string name) => new(name);
    
}
