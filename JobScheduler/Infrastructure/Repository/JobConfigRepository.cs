using System.Data;
using Dapper;
using JobScheduler.Domain.Job;
using JobScheduler.Infrastructure.Abstractions;

namespace JobScheduler.Infrastructure.Repository;

public class JobConfigRepository : IJobConfigRepository
{
    private readonly ISqlProvider _sqlProvider;

    public JobConfigRepository(ISqlProvider sqlProvider)
    {
        _sqlProvider = sqlProvider;
    }

    public async Task<long> AddAsync(JobConfig jobConfig)
    {
        const string sql = @"
            INSERT INTO JobConfig (
                Name, 
                Active, 
                CreatedTime, 
                UpdatedTime, 
                CreatedById, 
                UpdatedById)
            VALUES (@Name, 
                    @Active, 
                    @CreatedTime, 
                    @UpdatedTime, 
                    @CreatedById, 
                    @UpdatedById)
            RETURNING Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.ExecuteScalarAsync<long>(sql, jobConfig);
    }

    public async Task<JobConfig> GetByIdAsync(long id)
    {
        const string sql = "SELECT * FROM JobConfig WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<JobConfig>(sql, new { Id = id });
    }

    public async Task<IEnumerable<JobConfig>> GetAllAsync()
    {
        const string sql = "SELECT * FROM JobConfig;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.QueryAsync<JobConfig>(sql);
    }

    public async Task<bool> UpdateAsync(JobConfig jobConfig)
    {
        const string sql = @"
            UPDATE JobConfig
            SET Name = @Name,
                Active = @Active,
                CreatedTime = @CreatedTime,
                UpdatedTime = @UpdatedTime,
                CreatedById = @CreatedById,
                UpdatedById = @UpdatedById
            WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        int rowsAffected = await connection.ExecuteAsync(sql, jobConfig);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        const string sql = "DELETE FROM JobConfig WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        int rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }
}
