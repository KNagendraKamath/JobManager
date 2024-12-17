using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScheduler.Domain.Jobs;

namespace JobScheduler.Infrastructure.Interfaces;
public interface IJobRepository
{
    IEnumerable<Job> GetAll();
    JobConfig GetById(int id);
    void Add(JobConfig jobConfig);
    void Update(JobConfig jobConfig);
    void Delete(int id);

}
