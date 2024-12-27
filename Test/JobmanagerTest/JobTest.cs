using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScheduler.Domain.Job;
using JobScheduler.Infrastructure.Abstractions;
using JobScheduler.Infrastructure.Repository;
using Microsoft.Identity.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
      
            EffectiveDateTime = DateTime.UtcNow,
            Description = "Sample Job",
            Type = JobType.Recurring,
            RecurringType = RecurringType.Daily,
            Active = true,
            CreatedTime = DateTime.UtcNow,
            UpdatedTime = DateTime.UtcNow,
            CreatedById = 1,
            UpdatedById = 1
        }; 

        long id = await jobRepository.AddAsync(job);
        Assert.NotNull(id);
    }

    [Fact]
    public async Task GetAll()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        IJobRepository jobRepository = new JobRepository(sqlProvider);

        IEnumerable<Job> jobs = await jobRepository.GetAllAsync();
        Assert.NotEmpty(jobs);
    }

    [Fact]
    public async Task TestGetById()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        IJobRepository jobRepository = new JobRepository(sqlProvider);

        Job job =await jobRepository.GetByIdAsync(1);
        Assert.NotNull(job);
    }
    [Fact]
    public async Task TestUpdate()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        IJobRepository jobRepository = new JobRepository(sqlProvider);

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
        bool value = await jobRepository.UpdateAsync(job);
        Assert.True(value);

    }
    [Fact]
    public async Task TestDeleteById()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        IJobRepository jobRepository = new JobRepository(sqlProvider);

       bool value = await jobRepository.DeleteAsync(1);
        Assert.True(value);
    }
}
