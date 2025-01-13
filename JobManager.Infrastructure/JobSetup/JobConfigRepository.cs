
using System.Data;
using Dapper;
using JobManager.Application.Abstractions.Database;
using JobManager.Domain.JobSetup;

namespace JobManager.Infrastructure.JobSetup;

internal sealed class JobConfigRepository :  IJobConfigRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JobConfigRepository(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }


    public async Task<JobConfig> GetJobConfigAsync(string name, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        string query = "SELECT * FROM job_config WHERE Name = @Name";
        return await connection.QueryFirstOrDefaultAsync<JobConfig>(new CommandDefinition(query, new { Name = name }, cancellationToken: cancellationToken));
    }
}
