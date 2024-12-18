using System.Data;
using Dapper;
using JobScheduler.Domain.Models;
using JobScheduler.Infrastructure.Abstractions;

namespace JobScheduler.Infrastructure.Repository;
public class JobInstanceRepository
{
    private readonly ISqlProvider _sqlProvider;

    public JobInstanceRepository(ISqlProvider sqlProvider)
    {
        _sqlProvider = sqlProvider;
    }

    public async Task<long> AddAsync(JobInstance jobInstance)
    {
        const string sql = @"
            INSERT INTO JobInstance (
                JobId, 
                Status,
                Active, 
                CreatedTime, 
                UpdatedTime, 
                CreatedById, 
                UpdatedById)
            VALUES (@JobId, 
                    @Status,
                    @Active, 
                    @CreatedTime, 
                    @UpdatedTime, 
                    @CreatedById, 
                    @UpdatedById)
            RETURNING Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.ExecuteScalarAsync<long>(sql, jobInstance);
    }

    public async Task<JobInstance> GetByIdAsync(long id)
    {
        const string sql = "SELECT * FROM JobInstance WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<JobInstance>(sql, new { Id = id });
    }

    public async Task<IEnumerable<JobInstance>> GetAllAsync()
    {
        const string sql = "SELECT * FROM JobInstance;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.QueryAsync<JobInstance>(sql);
    }

    public async Task<bool> UpdateAsync(JobInstance jobInstance)
    {
        const string sql = @"
            UPDATE JobInstance
            SET JobId = @JobId,
                Status = @Status,
                Active = @Active,
                CreatedTime = @CreatedTime,
                UpdatedTime = @UpdatedTime,
                CreatedById = @CreatedById,
                UpdatedById = @UpdatedById
            WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        int rowsAffected = await connection.ExecuteAsync(sql, jobInstance);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        const string sql = "DELETE FROM JobInstance WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        int rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }
}
