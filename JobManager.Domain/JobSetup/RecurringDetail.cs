using JobManager.Framework.Domain.Abstractions;

namespace JobManager.Framework.Domain.JobSetup;

public class RecurringDetail:Entity
{
    public Job Job { get; private set; }
    public long JobId {  get; private set; }
    public RecurringType RecurringType { get; private set; }
    public int Second { get; private set; }
    public int Minute { get; private set; }
    public int Hours { get; private set; }
    public DayOfWeek DayOfWeek { get;private set; }
    public int Day { get; private set; }

    public RecurringDetail()
    {

    }

    public RecurringDetail(Job job,RecurringType recurringType, int second, int minute, int hours, DayOfWeek dayOfWeek, int day)
    {
        Job = job;
        JobId = job.Id;
        RecurringType = recurringType;
        Second = second;
        Minute = minute;
        Hours = hours;
        DayOfWeek = dayOfWeek;
        Day = day;
    }
}
