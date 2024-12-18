using JobScheduler.Domain.Jobs;

namespace JobScheduler.Infrastructure.Job;

public interface IJobConfigRepository
{
    IEnumerable<JobConfig> GetAll();
    JobConfig GetById(int id);
    void Add(JobConfig jobConfig);
    void Update(JobConfig jobConfig);
    void Delete(int id);
}
