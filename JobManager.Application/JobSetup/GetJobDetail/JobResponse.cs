
namespace JobManager.Application.JobSetup.GetJobDetail;
public sealed class JobResponse
{
    public long JobId { get; init;}
    public string Name { get; init;}
    public DateTime EffectiveDateTime { get; init;}
    public List<JobStepResponse> Steps { get; init; } = new();
}

public sealed class JobStepResponse
{
    public long JobStepId { get; init; }
    public string JobConfigName { get; init; }
    public string JsonParameter { get; init; }
}
