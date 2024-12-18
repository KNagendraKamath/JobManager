using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScheduler.Domain.Jobs;

namespace JobScheduler.Infrastructure.Job;
public interface IJobStepRepository
{
    IEnumerable<JobStep> GetAll();
    JobStep GetById(int id);
    void Add(JobStep Addjob);
    void Update(JobStep UpdateJob);
    void Delete(int id);
}
