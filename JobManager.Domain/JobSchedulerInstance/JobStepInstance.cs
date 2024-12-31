
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSetup;

namespace JobManager.Domain.JobSchedulerInstance;

public class JobStepInstance:Entity
{
    private JobStepInstance() { }

    private JobStepInstance(long jobInstanceId, long jobStepId, Status stepInstanceStatus)
    {
        JobInstanceId = jobInstanceId;
        JobStepId = jobStepId;
        Status = stepInstanceStatus;
        CreatedById = 1;
        CreatedTime = DateTime.UtcNow;
        Active = true;
    }

    public long JobInstanceId {  get; private set; }
    public JobInstance JobInstance { get; private set; }

    public long JobStepId { get; private set; }
    public JobStep JobStep {  get; private set; }

    public Status Status {  get;private set; }
    public DateTimeOffset StartTime { get; private set; }
    public DateTimeOffset? EndTime { get; private set; }
    public List<JobStepInstanceLog> JobStepInstanceLogs { get; private set; } = new();

    public static JobStepInstance Create(long jobInstanceId, long jobStepId, Status stepInstanceStatus)
    {
        return new JobStepInstance(jobInstanceId, jobStepId, stepInstanceStatus);
    }

    public void UpdateStatus(Status status)
    {
        Status = status;  
    }

    public void SetEndTime(DateTimeOffset endTime)
    {
        EndTime = endTime;
        Status = Status.Completed; 
    }

    public void SetStartTime(DateTimeOffset startTime)
    {
        StartTime = startTime;
        Status = Status.Running;
    }

    public void AddLog(string log)
    {
        JobStepInstanceLog logEntry = new JobStepInstanceLog(this.Id, log);
        JobStepInstanceLogs.Add(logEntry);
    }
}
