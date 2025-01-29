﻿namespace JobManager.Framework.Domain.JobSetup;

public interface IJobConfigRepository
{
    Task<JobConfig> GetJobConfigAsync(string name, CancellationToken cancellationToken=default);
    Task<IEnumerable<JobConfig>> GetJobConfigByNamesAsync(string[] jobNames, CancellationToken cancellationToken = default);
    Task AddJobConfig(IEnumerable<JobConfig> jobConfigs,CancellationToken cancellationToken=default);
}
