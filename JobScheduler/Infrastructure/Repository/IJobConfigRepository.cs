using JobScheduler.Domain.Jobs;

namespace JobScheduler.Infrastructure.Repository;

public interface IJobConfigRepository
{
    Task<IEnumerable<JobConfig>> GetAllAsync();
    Task<JobConfig> GetByIdAsync(long id);
    Task<long> AddAsync(JobConfig jobConfig);
    Task<bool> UpdateAsync(JobConfig jobConfig);
    Task<bool> DeleteAsync(long id);
}
