using JobManager.Domain.JobSchedulerInstance;
using JobManager.Infrastructure.Abstractions;

namespace JobManager.Infrastructure.JobSchedulerInstance;

internal sealed class JobStepInstanceRepository : Repository<JobStepInstance>, IJobStepInstanceRepository
{
    public JobStepInstanceRepository(JobDbContext dbContext) : base(dbContext)
    {
    }
}
