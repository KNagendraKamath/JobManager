﻿namespace JobManager.Framework.Domain.JobSetup;
public interface IJobRepository
{
    Task<long> AddAsync(Job job);
    Task<Job?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Job>> GetJobs(int page,int pageSize ,CancellationToken cancellationToken = default);
    Task DeactivateJobAsync(long jobId, CancellationToken cancellationToken = default);
    Task RemoveJobStep(long jobStepId, CancellationToken cancellationToken = default);
}
