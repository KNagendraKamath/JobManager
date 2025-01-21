
using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Application.JobSetup.GetJobDetail;
public sealed class JobResponse
{
    public long JobId { get; init; }
    public string Name { get; init; }
    public DateTime EffectiveDateTime { get; init; }
    public JobType JobType { get; init; }
    public string CronExpression { get; init; }
    public long? RecurringDetailId { get; init; }
    public RecurringType? RecurringType { get; init; }
    public int? Second { get; init; }
    public int? Minute { get; init; }
    public int? Hour { get; init; }
    public DayOfWeek? DayOfWeek { get; init; }
    public int? Day { get; init; }
    public List<JobStepResponse> Steps { get; init; }=new();
}

public sealed class JobStepResponse
{
    public long JobStepId { get; init; }
    public string JobConfigName { get; init; }
    public string Assembly { get; init; }
    public string JsonParameter { get; init; }
}
