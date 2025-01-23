using System.Data;
using System.Xml.Linq;
using Dapper;
using JobManager.Framework.Application.Abstractions.Database;
using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Infrastructure.JobSetup;

internal sealed class JobConfigRepository :  IJobConfigRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JobConfigRepository(ISqlConnectionFactory sqlConnectionFactory) => 
        _sqlConnectionFactory = sqlConnectionFactory;


    public async Task<JobConfig> GetJobConfigAsync(string name, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        string query = "SELECT * FROM job_config WHERE Name = @Name";
        return await connection.QueryFirstOrDefaultAsync<JobConfig>(new(query,
                                                                        new { Name = name },
                                                                        cancellationToken:cancellationToken));
    }

    public async Task<IEnumerable<JobConfig>> GetJobConfigByNamesAsync(string namesInCSV, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        string query = "SELECT * FROM job_config WHERE Name in (@NamesInCSV)";
        return (await connection.QueryAsync<JobConfig>(new(query,
                                                           new { NamesInCSV = namesInCSV },
                                                           cancellationToken:cancellationToken)));
    }
}
