using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Application.JobSetup.ScheduleJob;

public interface ICronExpressionGenerator
{
    string Generate(RecurringType? recurringType, int? second, int? minute, int? hour, int? day, DayOfWeek? dayOfWeek);
}
