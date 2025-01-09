namespace JobManager.Domain.JobSchedulerInstance;
public interface IJobScheduler
{
    Task ScheduleAsync(long GroupId,
                  long StepId,
                  string JobName,
                  string JsonParameter,
                  DateTime EffectiveDateTime,
                  string? cronExpression=default,
                  CancellationToken cancellationToken = default);

    Task UnSchedule(long GroupId,IEnumerable<long> StepId);

    Task ExecuteAsync(CancellationToken cancellationToken = default);
}
