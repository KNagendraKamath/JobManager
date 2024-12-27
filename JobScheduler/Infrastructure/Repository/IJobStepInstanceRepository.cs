using JobScheduler.Domain.JobInstance;

namespace JobScheduler.Infrastructure.Repository;

public interface IJobStepInstanceRepository
{
    Task<IEnumerable<JobStepInstance>> GetAllAsync();
    Task<JobStepInstance> GetByIdAsync(long id);
    Task<long> AddAsync(JobStepInstance jobStepInstance);
    Task<bool> UpdateAsync(JobStepInstance jobStepInstance);
    Task<bool> DeleteAsync(long id);
}
