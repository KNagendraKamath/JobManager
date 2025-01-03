using System.Text.Json.Serialization;
using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.JobSetup;

namespace JobManager.Application.JobSetup.CreateJob;

public record CreateJobCommand(
    string? Description,
    DateTime EffectiveDateTime,
    JobType JobType,
    RecurringDetail? RecurringDetail,
    List<Step> JobSteps) : ICommand<long>;

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

    public RecurringDetail()
    {

    }
}


