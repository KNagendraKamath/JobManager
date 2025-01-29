using JobManager.Framework.Domain.Abstractions;

namespace JobManager.Framework.Domain.JobSetup;

public class RecurringDetail:Entity
{
    public Job Job { get; private set; }
    public long JobId {  get; private set; }
    public string RecurringType { get; private set; }
    public int Second { get; private set; }
    public int Minute { get; private set; }
    public int Hour { get; private set; }
    public string DayOfWeek { get;private set; }
    public int Day { get; private set; }

    public RecurringDetail()
    {

    }

    public RecurringDetail(Job job,
                           string recurringType,
                           int second,
                           int minute,
                           int hour,
                           string dayOfWeek,
                           int day)
    {
        Job = job;
        JobId = job.Id;
        RecurringType = recurringType;
        Second = second;
        Minute = minute;
        Hour = hour;
        DayOfWeek = dayOfWeek;
        Day = day;
    }
}
