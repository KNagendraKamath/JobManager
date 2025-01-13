using JobManager.Application.Abstractions.Database;
using System.Data;
using JobManager.Domain.JobSetup;
using Dapper;
using Job = JobManager.Domain.JobSetup.Job;

namespace JobManager.Infrastructure.JobSetup;
internal sealed class JobRepository : IJobRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JobRepository(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<long> AddAsync(Job job)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            INSERT INTO job(
                Description, 
                EffectiveDateTime, 
                Type, 
                RecurringType, 
                CreatedTime,  
                CreatedById, 
                Active
            )
            VALUES (
                @Description, 
                @EffectiveDateTime, 
                @Type, 
                @RecurringType, 
                @CreatedTime, 
                @CreatedById,  
                @Active
            )
            RETURNING Id;";

        job.Id = await connection.QueryFirstOrDefaultAsync<long>(sql, new
        {
            job.Description,
            job.EffectiveDateTime,
            job.Type,
            job.CreatedTime,
            job.CreatedById,
            job.Active
        });

        return job.Id;
    }

    public async Task DeactivateJobAsync(long jobId, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            UPDATE job
            SET 
                Active = 0,
                UpdatedTime = @UpdatedTime,
                UpdatedById = @UpdatedById
            WHERE Id = @Id;";

        await connection.ExecuteAsync(sql, new
        {
            Id = jobId,
            UpdatedTime = DateTimeOffset.UtcNow,
            UpdatedById = 1 // Assuming a default user ID for the update
        });
    }

    public async Task<Job?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT Id, Description, EffectiveDateTime, Type, RecurringType, CreatedTime, UpdatedTime, CreatedById, UpdatedById, Active
            FROM job
            WHERE Id = @Id;";

        return await connection.QuerySingleOrDefaultAsync<Job>(sql, new { Id = id });
    }

    public async Task RemoveJobStep(long jobStepId, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            UPDATE job_step
            SET 
                Active = 0,
                UpdatedTime = @UpdatedTime,
                UpdatedById = @UpdatedById
            WHERE Id = @Id;";

        await connection.ExecuteAsync(sql, new
        {
            Id = jobStepId,
            UpdatedTime = DateTimeOffset.UtcNow,
            UpdatedById = 1 // Assuming a default user ID for the update
        });
    }
}
