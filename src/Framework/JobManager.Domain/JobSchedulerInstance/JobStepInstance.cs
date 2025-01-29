using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Domain.JobSchedulerInstance;

public class JobStepInstance:Entity
{
    private JobStepInstance() { }

    private JobStepInstance(long jobInstanceId, long jobStepId, string stepInstanceStatus)
    {
        JobInstanceId = jobInstanceId;
        JobStepId = jobStepId;
        Status = stepInstanceStatus;
    }

    public long JobInstanceId {  get; private set; }
    public JobInstance JobInstance { get; private set; }

    public long JobStepId { get; private set; }
    public JobStep JobStep {  get; private set; }

    public string Status {  get;private set; }
    public DateTimeOffset? StartTime { get; private set; }
    public DateTimeOffset? EndTime { get; private set; }
    public List<JobStepInstanceLog> JobStepInstanceLogs { get; private set; } = new();

    public static JobStepInstance Create(long jobInstanceId, long jobStepId, string stepInstanceStatus) =>
        new JobStepInstance(jobInstanceId, jobStepId, stepInstanceStatus);

    public void UpdateStatus(string status) => Status = status;

    public void SetEndTime(DateTimeOffset endTime,string status)
    {
        EndTime = endTime;
        Status = status;
    }

    public void SetStartTime(DateTimeOffset startTime,string status)
    {
        StartTime = startTime;
        Status = status;
    }

    public void AddLog(string log)
    {
        JobStepInstanceLog logEntry = JobStepInstanceLog.Create(this.Id, log);
        JobStepInstanceLogs.Add(logEntry);
    }
}
