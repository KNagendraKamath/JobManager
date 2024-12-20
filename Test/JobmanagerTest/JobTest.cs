using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScheduler.Domain.Jobs;
using JobScheduler.Infrastructure.Abstractions;
using JobScheduler.Infrastructure.Repository;
using Microsoft.Identity.Client;

namespace JobmanagerTest;
public class JobTest
{
    [Fact]
    public async Task TestAddAsync()
    {
        ISqlProvider sqlProvider= new PostGresSqlProvider();
        IJobRepository jobRepository= new JobRepository(sqlProvider);

        Job job = new Job
        {
            Id = 1,
            EffectiveDateTime = DateTime.UtcNow,
            Description = "new Job",
            Type = JobType.Recurring,
            RecurringType = RecurringType.Daily,
            Active = true,
            CreatedTime = DateTimeOffset.UtcNow,
            UpdatedTime = DateTimeOffset.UtcNow,
            CreatedById = 2,
            UpdatedById = 2
        };
        long id= await jobRepository.AddAsync(job);
        Assert.Equal(job.Id, id);

    }
    [Fact]
    public async Task GetAll()
}
