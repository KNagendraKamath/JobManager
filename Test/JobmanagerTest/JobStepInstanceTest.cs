using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScheduler.Domain.Models;
using JobScheduler.Infrastructure.Abstractions;
using JobScheduler.Infrastructure.Repository;

namespace JobmanagerTest;
public class JobStepInstanceTest
{
    [Fact]
    public async Task TestAddAsync()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobStepInstanceRepository repository = new JobStepInstanceRepository(sqlProvider);

        JobStepInstance newJobInstance = new JobStepInstance
        {
           
            JobInstanceId=3,
            JobStatus=Status.Running,
            StartTime=DateTimeOffset.UtcNow,
            EndTime=DateTimeOffset.UtcNow,
            Active=true,
            CreatedTime=DateTimeOffset.UtcNow,
            UpdatedTime=DateTimeOffset.UtcNow,
            CreatedById=1,
            UpdatedById=1
        };
        long id = await repository.AddAsync(newJobInstance);
        Assert.NotNull(id);
    }

    [Fact]
    public async Task TestGetByIdAsync()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobInstanceRepository repository = new JobInstanceRepository(sqlProvider);

        long id = 3;

        JobInstance retrievedJobInstance = await repository.GetByIdAsync(id);
        Assert.NotNull(retrievedJobInstance);
        

    }

    [Fact]
    public async Task TestGetAllAsync()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobInstanceRepository repository = new JobInstanceRepository(sqlProvider);

        IEnumerable<JobInstance> retrievedJobInstance = await repository.GetAllAsync();
        Assert.NotNull(retrievedJobInstance);
    }

    [Fact]
    public async Task TestUpdateAsync()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobInstanceRepository repository = new JobInstanceRepository(sqlProvider);

        JobInstance newJobInstance = new JobInstance
        {
            Id = 1,
            JobId = 1,
            JobStatus = Status.Running,
            Active = true,
            CreatedTime = DateTimeOffset.UtcNow,
            UpdatedTime = DateTimeOffset.UtcNow,
            CreatedById = 1,
            UpdatedById = 1
        };
        Boolean value = await repository.UpdateAsync(newJobInstance);
        Assert.True(value);

    }
    [Fact]
    public async Task TestDeleteById()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobInstanceRepository repository = new JobInstanceRepository(sqlProvider);

        bool value = await repository.DeleteAsync(1);
        Assert.True(value);

    }


}
