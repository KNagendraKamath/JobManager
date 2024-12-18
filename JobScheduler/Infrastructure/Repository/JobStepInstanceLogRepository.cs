using System.Data;
using Dapper;
using JobScheduler.Domain.Models;
using JobScheduler.Infrastructure.Abstractions;

namespace JobScheduler.Infrastructure.Repository;

public class JobStepInstanceLogRepository:IJobStepInstanceLogRepository
{
    private readonly ISqlProvider _sqlProvider;

    public JobStepInstanceLogRepository(ISqlProvider sqlProvider)
    {
        _sqlProvider = sqlProvider;
    }

    public async Task<long> AddAsync(JobStepInstanceLog jobStepInstanceLog)
    {
        const string sql = @"
            INSERT INTO JobStepInstanceLog (
                JobStepInstanceId, 
                Log, 
                Active, 
                CreatedTime, 
                UpdatedTime, 
                CreatedById, 
                UpdatedById)
            VALUES (@JobStepInstanceId, 
                    @Log, 
                    @Active, 
                    @CreatedTime, 
                    @UpdatedTime, 
                    @CreatedById, 
                    @UpdatedById)
            RETURNING Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.ExecuteScalarAsync<long>(sql, jobStepInstanceLog);
    }

    public async Task<JobStepInstanceLog> GetByIdAsync(long id)
    {
        const string sql = "SELECT * FROM JobStepInstanceLog WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<JobStepInstanceLog>(sql, new { Id = id });
    }

    public async Task<IEnumerable<JobStepInstanceLog>> GetAllAsync()
    {
        const string sql = "SELECT * FROM JobStepInstanceLog;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.QueryAsync<JobStepInstanceLog>(sql);
    }

    public async Task<bool> UpdateAsync(JobStepInstanceLog jobStepInstanceLog)
    {
        const string sql = @"
            UPDATE JobStepInstanceLog
            SET JobStepInstanceId = @JobStepInstanceId,
                Log = @Log,
                Active = @Active,
                CreatedTime = @CreatedTime,
                UpdatedTime = @UpdatedTime,
                CreatedById = @CreatedById,
                UpdatedById = @UpdatedById
            WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        int rowsAffected = await connection.ExecuteAsync(sql, jobStepInstanceLog);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        const string sql = "DELETE FROM JobStepInstanceLog WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        int rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }
}
