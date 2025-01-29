using System.Data;
using Dapper;
using JobManager.Framework.Application.Abstractions.Database;
using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Infrastructure.JobSetup;

internal sealed class JobConfigRepository :  IJobConfigRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JobConfigRepository(ISqlConnectionFactory sqlConnectionFactory) => 
        _sqlConnectionFactory = sqlConnectionFactory;

    public async Task AddJobConfig(IEnumerable<JobConfig> jobConfigs, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"INSERT INTO JOB.job_config(
                             Name,
                             Created_Time,
                             Active
                             )VALUES(
                             @Name,
                             @CreatedTime,
                             true
                            )
                            ON CONFLICT (Name) DO NOTHING";

        await connection.ExecuteAsync(sql, jobConfigs);
    }

    public async Task<JobConfig> GetJobConfigAsync(string name, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string query = @"SELECT id Id,
                                      name Name,
                                      created_time CreatedTime,
                                      updated_time UpdatedTime,     
                                      active Active
                               FROM JOB.job_config 
                               WHERE Name = @Name";

        return await connection.QueryFirstOrDefaultAsync<JobConfig>(new(query,
                                                                        new { Name = name },
                                                                        cancellationToken:cancellationToken));
    }

    public async Task<IEnumerable<JobConfig>> GetJobConfigByNamesAsync(string[] jobNames, CancellationToken cancellationToken = default)
    {
        try
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

            const string query = @"SELECT id ""Id"",
                                      name ""Name"",
                                      created_time ""CreatedTime"",
                                      updated_time ""UpdatedTime"",     
                                      active ""Active""
                               FROM JOB.job_config 
                               WHERE Name = ANY(@NamesInCSV)";


            IEnumerable<JobConfig> result = await connection.QueryAsync<JobConfig>(query,
                                                               new { NamesInCSV = jobNames });
            return result;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }
}
