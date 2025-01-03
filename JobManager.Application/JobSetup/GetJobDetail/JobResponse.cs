
using JobManager.Domain.JobSetup;

namespace JobManager.Application.JobSetup.GetJobDetail;
public sealed class JobResponse
{
    public long JobId { get; init; }
    public string Name { get; init; }
    public DateTime EffectiveDateTime { get; init; }
    public JobType JobType { get; init; }
    public long JobStepId { get; init; }
    public string JobConfigName { get; init; }
    public string JsonParameter { get; init; }
    public RecurringDetailResponse? RecurringDetail { get; private set; }

    public void SetRecurringDetail(RecurringDetailResponse recurringDetail)
    {
        RecurringDetail = recurringDetail;
    }
}

public sealed class RecurringDetailResponse
{
    public long? RecurringDetailId { get; init; }
    public RecurringType? RecurringType { get; init; }
    public int? Second { get; init; }
    public int? Minutes { get; init; }
    public int? Hours { get; init; }
    public DayOfWeek? DayOfWeek { get; init; }
    public int? Day { get; init; }
}
