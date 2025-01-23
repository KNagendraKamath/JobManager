using System.Globalization;
using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Application.JobSetup.ScheduleJob;

public class QuartzCronExpressionGenerator: ICronExpressionGenerator
{

    public string Generate(RecurringType? recurringType, int? second, int? minute, int? hour, int? day, DayOfWeek? dayOfWeek)
    {
        if (recurringType is null)
            return null;

        return recurringType switch
        {
            RecurringType.EveryNoSecond => $"0/{second ?? 1} * * * * ?", // Every N seconds
            RecurringType.EveryNoMinute => $"{second ?? 0} 0/{minute ?? 1} * * * ?", // Every N minutes
            RecurringType.Daily => $"{second ?? 0} {minute ?? 0} {hour ?? 0} * * ?", // Every day at a specific time
            RecurringType.Weekly => $"{second ?? 0} {minute ?? 0} {hour ?? 0} ? * {dayOfWeek?.ToString().ToUpper(CultureInfo.InvariantCulture).Substring(0, 3)}", // Every week on a specific day and time
            RecurringType.Monthly => $"{second ?? 0} {minute ?? 0} {hour ?? 0} {day ?? 1} * ?", // Every month on a specific day and time
            _ => throw new NotImplementedException($"Recurring type {recurringType} is not supported")
        };
    }
}
