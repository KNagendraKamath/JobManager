using JobScheduler.Domain.Jobs;

namespace JobScheduler.Infrastructure.Repository;
public interface IJobRepository
{
    Task<IEnumerable<Job>> GetAllAsync();
    Task<Job> GetByIdAsync(long id);
    Task<long> AddAsync(Job job);
    Task<bool> UpdateAsync(Job job);
    Task<bool> DeleteAsync(long id);

}
