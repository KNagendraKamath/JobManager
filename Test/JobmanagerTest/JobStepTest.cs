using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using JobScheduler.Domain.Job;
using JobScheduler.Infrastructure.Abstractions;
using JobScheduler.Infrastructure.Repository;

namespace JobmanagerTest;
public class JobStepTest
{
    [Fact]
    public async Task TestAddJob()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobStepRepository repository = new JobStepRepository(sqlProvider);

        JobStep newJobConfig = new JobStep
        {
                Id=1,
                JobId=1,
                JobConfigId=1234,
                Parameter="dsfgrfgfdge",
                Active=true,
                CreatedTime=DateTimeOffset.UtcNow,
                UpdatedTime=DateTimeOffset.UtcNow,
                CreatedById=2,
                UpdatedById=2
        };
        long id = await repository.AddAsync(newJobConfig);
        Assert.Equal(newJobConfig.Id, id);
    }

    [Fact]
    public async Task TestGetByIdAsync()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobStepRepository repository = new JobStepRepository(sqlProvider);

        long id = 1;

        JobStep retrievedJobConfig = await repository.GetByIdAsync(id);
        Assert.NotNull(retrievedJobConfig);
        Assert.Equal(id, retrievedJobConfig.Id);

    }

    [Fact]
    public async Task TestGetAllAsync()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobStepRepository repository = new JobStepRepository(sqlProvider);

        IEnumerable<JobStep> retrivedJobConfig = await repository.GetAllAsync();
        Assert.NotNull(retrivedJobConfig);
    }

    [Fact]
    public async Task TestUpdateAsync()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobStepRepository repository = new JobStepRepository(sqlProvider);

        JobStep newJobConfig = new JobStep
        {
            Id = 1,
            JobId = 1,
            JobConfigId = 1234,
            Parameter = "dsfgrfgfdge",
            Active = true,
            CreatedTime = DateTimeOffset.UtcNow,
            UpdatedTime = DateTimeOffset.UtcNow,
            CreatedById = 2,
            UpdatedById = 2
        };
        bool value = await repository.UpdateAsync(newJobConfig);
        Assert.True(value);

    }
    [Fact]
    public async Task TestDeleteById()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobStepRepository repository = new JobStepRepository(sqlProvider);
        

        bool value = await repository.DeleteAsync(1);
        Assert.True(value);

    }



}
