using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScheduler.Infrastructure.Abstractions;

namespace JobScheduler.Infrastructure.Job;
public class JobRepository
{
    private readonly JobDapper _jobDapper;
    public JobRepository(JobDapper jobDapper)
    {
        _jobDapper = jobDapper;
    }
}
