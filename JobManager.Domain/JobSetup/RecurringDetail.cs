using JobManager.Domain.Abstractions;

namespace JobManager.Domain.JobSetup;

public class RecurringDetail:Entity
{
    public Job Job { get; private set; }
    public long JobId {  get; private set; }
    public RecurringType RecurringType { get; private set; }
    public int Second { get; private set; }
    public int Minutes { get; private set; }
    public int Hours { get; private set; }
    public DayOfWeek DayOfWeek { get;private set; }
    public int Day { get; private set; }

    public RecurringDetail()
    {

    }

    public RecurringDetail(Job job,RecurringType recurringType, int second, int minutes, int hours, DayOfWeek dayOfWeek, int day)
    {
        Job = job;
        JobId = job.Id;
        RecurringType = recurringType;
        Second = second;
        Minutes = minutes;
        Hours = hours;
        DayOfWeek = dayOfWeek;
        Day = day;
    }
}
