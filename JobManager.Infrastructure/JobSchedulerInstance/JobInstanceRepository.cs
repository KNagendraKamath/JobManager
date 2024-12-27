using JobManager.Domain.JobSchedulerInstance;
using JobManager.Infrastructure.Abstractions;

namespace JobManager.Infrastructure.JobSchedulerInstance;

internal sealed class JobInstanceRepository : Repository<JobInstance>, IJobInstanceRepository
{
    public JobInstanceRepository(JobDbContext context):base(context)
    {
    }
}
