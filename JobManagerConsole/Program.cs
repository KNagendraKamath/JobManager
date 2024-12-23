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

       
    }
}
