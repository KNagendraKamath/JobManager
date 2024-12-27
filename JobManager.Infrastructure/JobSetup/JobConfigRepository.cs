
using JobManager.Domain.JobSetup;
using JobManager.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace JobManager.Infrastructure.JobSetup;

internal sealed class JobConfigRepository : Repository<JobConfig>, IJobConfigRepository
{
    public JobConfigRepository(JobDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<JobConfig> GetJobConfigAsync(string name, CancellationToken cancellationToken=default) => 
        await DbContext.Set<JobConfig>().FirstOrDefaultAsync(u => u.Name == name, cancellationToken);

}
