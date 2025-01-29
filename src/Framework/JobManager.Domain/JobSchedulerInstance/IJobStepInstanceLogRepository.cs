namespace JobManager.Framework.Domain.JobSchedulerInstance;
public interface IJobStepInstanceLogRepository
{
    Task AddAsync(JobStepInstanceLog jobStepInstanceLog);

}
