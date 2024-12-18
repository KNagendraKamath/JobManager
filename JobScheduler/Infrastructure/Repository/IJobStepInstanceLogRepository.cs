using JobScheduler.Domain.Models;

namespace JobScheduler.Infrastructure.Repository;
internal interface IJobStepInstanceLogRepository
{
    Task<IEnumerable<JobStepInstanceLog>> GetAllAsync();
    Task<JobStepInstanceLog> GetByIdAsync(long id);
    Task<long> AddAsync(JobStepInstanceLog jobStepInstanceLog);
    Task<bool> UpdateAsync(JobStepInstanceLog jobStepInstanceLog);
    Task<bool> DeleteAsync(long id);
}
