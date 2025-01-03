using JobManager.Domain.Abstractions;

namespace JobManager.Domain.JobSetup;

public class JobStep:Entity
{
    private JobStep() { }

    public JobStep(Job job,JobConfig jobConfig, string jsonParameter)
    {
        Job = job;
        JobId = job.Id;
        JobConfigId = jobConfig.Id;
        JobConfig = jobConfig;
        JsonParameter = jsonParameter;
    }

    public long JobId { get; private set; }
    public Job Job { get; private set; }
    public long JobConfigId { get; private set; }
    public JobConfig JobConfig { get; private set;}
    public string JsonParameter { get; private set; }
}
