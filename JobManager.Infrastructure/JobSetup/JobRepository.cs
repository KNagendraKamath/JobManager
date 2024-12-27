using JobManager.Domain.JobSetup;
using JobManager.Infrastructure.Abstractions;

namespace JobManager.Infrastructure.JobSetup;

internal sealed class JobRepository : Repository<Job>, IJobRepository
{
    public JobRepository(JobDbContext context) : base(context) { }
}
