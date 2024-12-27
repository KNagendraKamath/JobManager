using JobManager.Domain.JobSchedulerInstance;
using JobManager.Infrastructure.Abstractions;

namespace JobManager.Infrastructure.JobSchedulerInstance;

internal sealed class JobStepInstanceLogRepository : Repository<JobStepInstanceLog>, IJobStepInstanceLogRepository
{
    public JobStepInstanceLogRepository(JobDbContext context) : base(context)
    {
    }
}
