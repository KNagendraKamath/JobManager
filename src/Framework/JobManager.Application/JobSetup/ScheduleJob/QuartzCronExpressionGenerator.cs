using System.Globalization;

namespace JobManager.Framework.Application.JobSetup.ScheduleJob;

public class QuartzCronExpressionGenerator: ICronExpressionGenerator
{
    private const string EveryNoSecond = "EveryNoSecond";
    private const string EveryNoMinute = "EveryNoMinute";
    private const string Daily = "Daily";
    private const string Weekly = "Weekly";
    private const string Monthly = "Monthly";

    public string Generate(string? recurringType, int? second, int? minute, int? hour, int? day, DayOfWeek? dayOfWeek)
    {
        if (recurringType is null)
            return null;

        return recurringType switch
        {
            EveryNoSecond => $"0/{second ?? 1} * * * * ?", // Every N seconds
            EveryNoMinute => $"{second ?? 0} 0/{minute ?? 1} * * * ?", // Every N minutes
            Daily => $"{second ?? 0} {minute ?? 0} {hour ?? 0} * * ?", // Every day at a specific time
            Weekly => $"{second ?? 0} {minute ?? 0} {hour ?? 0} ? * {dayOfWeek?.ToString().ToUpper(CultureInfo.InvariantCulture).Substring(0, 3)}", // Every week on a specific day and time
            Monthly => $"{second ?? 0} {minute ?? 0} {hour ?? 0} {day ?? 1} * ?", // Every month on a specific day and time
            _ => throw new NotImplementedException($"Recurring type {recurringType} is not supported")
        };
    }
}
