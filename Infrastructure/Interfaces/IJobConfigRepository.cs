
using JobScheduler.Domain.Jobs;

namespace JobScheduler.Infrastructure.Interfaces;

public interface IJobConfigRepository
{
    IEnumerable<Job> GetAll();
    Job GetById(int id);
    void Add(JobConfig jobConfig);
    void Update(JobConfig jobConfig);
    void Delete(int id);
}
