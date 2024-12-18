using JobScheduler.Domain.Jobs;

namespace JobScheduler.Infrastructure.Repository;
public interface IJobStepRepository
{
    Task<IEnumerable<JobStep>> GetAllAsync();
    Task<JobStep> GetByIdAsync(long id);
    Task<long> AddAsync(JobStep jobStep);
    Task<bool> UpdateAsync(JobStep job);
    Task<bool> DeleteAsync(long id);
}
