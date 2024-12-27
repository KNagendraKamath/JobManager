using System.Data;
using Dapper;
using JobScheduler.Domain.Job;
using JobScheduler.Infrastructure.Abstractions;

namespace JobScheduler.Infrastructure.Repository;

public class JobStepRepository : IJobStepRepository
{
    private readonly ISqlProvider _sqlProvider;

    public JobStepRepository(ISqlProvider sqlProvider)
    {
        _sqlProvider = sqlProvider;
    }

    public async Task<long> AddAsync(JobStep jobStep)
    {
        const string sql = @"
            INSERT INTO JobStep (
                JobId, 
                JobConfigId, 
                Parameter, 
                Active, 
                CreatedTime, 
                UpdatedTime, 
                CreatedById, 
                UpdatedById)
            VALUES (@JobId, 
                    @JobConfigId, 
                    @Parameter, 
                    @Active, 
                    @CreatedTime, 
                    @UpdatedTime, 
                    @CreatedById, 
                    @UpdatedById)
            RETURNING Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.ExecuteScalarAsync<long>(sql, jobStep);
    }

    public async Task<JobStep> GetByIdAsync(long id)
    {
        const string sql = "SELECT * FROM JobStep WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<JobStep>(sql, new { Id = id });
    }

    public async Task<IEnumerable<JobStep>> GetAllAsync()
    {
        const string sql = "SELECT * FROM JobStep;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.QueryAsync<JobStep>(sql);
    }

    public async Task<bool> UpdateAsync(JobStep jobStep)
    {
        const string sql = @"
            UPDATE JobStep
            SET JobId = @JobId,
                JobConfigId = @JobConfigId,
                Parameter = @Parameter,
                Active = @Active,
                CreatedTime = @CreatedTime,
                UpdatedTime = @UpdatedTime,
                CreatedById = @CreatedById,
                UpdatedById = @UpdatedById
            WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        int rowsAffected = await connection.ExecuteAsync(sql, jobStep);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        const string sql = "DELETE FROM JobStep WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        int rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }
}

