using System.Data;
using Dapper;
using JobScheduler.Domain.Jobs;
using JobScheduler.Infrastructure.Abstractions;

namespace JobScheduler.Infrastructure.Repository;

public class JobRepository:IJobRepository
{
    private readonly ISqlProvider _sqlProvider;
    public JobRepository(ISqlProvider sqlProvider)
    {
        _sqlProvider = sqlProvider;
    }

    public async Task<long> AddAsync(Job job)
    {
        const string sql = @"
            INSERT INTO Job (
                Id,
                EffectiveDateTime, 
                Description, 
                Type, 
                RecurringType, 
                Active, 
                CreatedTime, 
                UpdatedTime, 
                CreatedById, 
                UpdatedById)
            VALUES (
                    @Id,
                    @EffectiveDateTime, 
                    @Description, 
                    @Type, 
                    @RecurringType, 
                    @Active, 
                    @CreatedTime, 
                    @UpdatedTime, 
                    @CreatedById, 
                    @UpdatedById)
            RETURNING Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.ExecuteScalarAsync<long>(sql, job);
    }

    public async Task<Job> GetByIdAsync(long id)
    {
        const string sql = "SELECT * FROM Job WHERE Id = @Id;";

        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Job>(sql, new { Id = id });
        
    }

    public async Task<IEnumerable<Job>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Job;";

        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.QueryAsync<Job>(sql);
        
    }

    public async Task<bool> UpdateAsync(Job job)
    {
        const string sql = @"
            UPDATE Job
            SET EffectiveDateTime = @EffectiveDateTime,
                Description = @Description,
                Type = @Type,
                RecurringType = @RecurringType,
                Active = @Active,
                CreatedTime = @CreatedTime,
                UpdatedTime = @UpdatedTime,
                CreatedById = @CreatedById,
                UpdatedById = @UpdatedById
            WHERE Id = @Id;";

        using IDbConnection connection = _sqlProvider.CreateConnection();
        int rowsAffected = await connection.ExecuteAsync(sql, job);
        return rowsAffected > 0;
        
    }

    public async Task<bool> DeleteAsync(long id)
    {
        const string sql = "DELETE FROM Job WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        int rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
        
    }
}
