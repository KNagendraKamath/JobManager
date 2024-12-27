using JobManager.Domain.JobSetup;
using JobManager.Infrastructure.Abstractions;

namespace JobManager.Infrastructure.JobSetup;

internal sealed class JobStepRepository : Repository<JobStep>, IJobStepRepository
{
    public JobStepRepository(JobDbContext dbContext) : base(dbContext)
    {
    }
}
