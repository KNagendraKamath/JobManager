

namespace JobManager.Framework.Application.JobSetup.ScheduleJob;

public interface ICronExpressionGenerator
{
    string Generate(string? recurringType, int? second, int? minute, int? hour, int? day, DayOfWeek? dayOfWeek);
}
