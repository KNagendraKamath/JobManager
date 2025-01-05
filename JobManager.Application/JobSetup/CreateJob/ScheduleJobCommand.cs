using System.Text.Json.Serialization;
using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.JobSetup;

namespace JobManager.Application.JobSetup.CreateJob;

public record ScheduleJobCommand(
    string? Description,
    DateTime EffectiveDateTime,
    JobType JobType,
    List<Step> JobSteps,
    RecurringDetail? RecurringDetail = default) : ICommand<long>;

public record Step(string JobName,string JsonParameter);

public record RecurringDetail
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RecurringType RecurringType { get; set; }

    public int Second { get; set; }
    public int Minutes { get; set; }
    public int Hours { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DayOfWeek DayOfWeek { get; set; }

    public int Day { get; set; }

    public RecurringDetail(RecurringType recurringType,
                           int? second,
                           int? minute,
                           int? hour,
                           int? day,
                           DayOfWeek? dayOfWeek)
    {
        RecurringType = recurringType;
        Second = second??0;
        Minutes = minute??0;
        Hours = hour??0;
        Day = day??1;
        DayOfWeek = dayOfWeek??DayOfWeek.Sunday;
    }
}


