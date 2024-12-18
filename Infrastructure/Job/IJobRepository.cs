using JobScheduler.Domain.Jobs;

namespace JobScheduler.Infrastructure.Job;
public interface IJobRepository
{
    IEnumerable<Jobs> GetAll();
    Jobs GetById(int id);
    void Add(Jobs Addjob);
    void Update(Jobs UpdateJob);
    void Delete(int id);

}
