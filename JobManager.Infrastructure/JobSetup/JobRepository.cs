using JobManager.Domain.JobSetup;
using JobManager.Infrastructure.Abstractions;

namespace JobManager.Infrastructure.JobSetup;

internal sealed class JobRepository : Repository<Job>, IJobRepository
{
    public JobRepository(JobDbContext context) : base(context) { }

    public async Task DeactivateJob(long jobId, CancellationToken cancellationToken = default)
    {
       await DeactivateAsync(jobId, cancellationToken);
    }

    public Task RemoveJobStep(long jobStepId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
