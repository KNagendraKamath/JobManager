using Autofac;
using JobScheduler.Domain.Jobs;
using JobScheduler.Infrastructure.Abstractions;
using JobScheduler.Infrastructure.Repository;
using Microsoft.IdentityModel.Tokens;

namespace JobManagerConsole;

internal class Program
{
    static async Task  Main(string[] args)
    {
        PostGresSqlProvider SqlConnection = new PostGresSqlProvider();
        ContainerBuilder builder = new ContainerBuilder();

        builder.RegisterInstance(new JobRepository(SqlConnection)).As<IJobRepository>();

        ISqlProvider sqlProvider = new PostGresSqlProvider();
        JobConfigRepository repository = new JobConfigRepository(sqlProvider);

        JobConfig newJobConfig = new JobConfig
        {
            Id=1234,
            Name = "Test Job",
            Active = true,
            CreatedTime = DateTime.UtcNow,
            UpdatedTime = DateTime.UtcNow,
            CreatedById = 1,
            UpdatedById = 1
        };

        try
        {
            long id = await repository.AddAsync(newJobConfig);
            Console.WriteLine($"New JobConfig ID: {id}");

            JobConfig retrievedJobConfig = await repository.GetByIdAsync(id);
            Console.WriteLine($"Retrieved JobConfig: {retrievedJobConfig.Name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
