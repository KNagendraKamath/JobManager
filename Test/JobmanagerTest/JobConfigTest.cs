using System.Data;
using System.Text.Json;
using Dapper;
using JobScheduler.Application.Abstractions;
using JobScheduler.Domain.Job;
using JobScheduler.Infrastructure.Abstractions;
using JobScheduler.Infrastructure.Repository;


namespace JobmanagerTest;

public class JobConfigTest
{

    [Fact]
    public async Task TestAddAsync()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobConfigRepository repository = new JobConfigRepository(sqlProvider);

        JobConfig newJobConfig = new JobConfig
        {
            Id = 123456,
            Name = "Test Job1",
            Active = true,
            CreatedTime = DateTime.UtcNow,
            UpdatedTime = DateTime.UtcNow,
            CreatedById = 1,
            UpdatedById = 1
        };
        long id = await repository.AddAsync(newJobConfig);
        Assert.Equal(newJobConfig.Id, id);
    }

    [Fact]
    public async Task TestGetByIdAsync()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobConfigRepository repository = new JobConfigRepository(sqlProvider);

        long id = 1234;

        JobConfig retrievedJobConfig = await repository.GetByIdAsync(id);
        Assert.NotNull(retrievedJobConfig);
        Assert.Equal(id, retrievedJobConfig.Id);

    }

    [Fact]
    public async Task TestGetAllAsync()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobConfigRepository repository = new JobConfigRepository(sqlProvider);

        IEnumerable<JobConfig> retrivedJobConfig = await repository.GetAllAsync();
        Assert.NotNull(retrivedJobConfig);
    }

    [Fact]
    public async Task TestUpdateAsync()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobConfigRepository repository = new JobConfigRepository(sqlProvider);

        JobConfig newJobConfig = new JobConfig
        {
            Id = 123456,
            Name = "Test Job1",
            Active = true,
            CreatedTime = DateTime.UtcNow,
            UpdatedTime = DateTime.UtcNow,
            CreatedById = 1,
            UpdatedById = 1
        };
        Boolean value = await repository.UpdateAsync(newJobConfig);

    }
    [Fact]
    public async Task TestDeleteById()
    {
        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobConfigRepository repository = new JobConfigRepository(sqlProvider);

        bool value= await repository.DeleteAsync(0);
        Assert.True(value);

    }
   

}
    
     

