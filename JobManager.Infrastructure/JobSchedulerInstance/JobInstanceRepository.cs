using System.Data;
using Dapper;
using JobManager.Application.Abstractions.Database;
using JobManager.Domain.JobSchedulerInstance;

namespace JobManager.Infrastructure.JobSchedulerInstance;

internal sealed class JobInstanceRepository : IJobInstanceRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JobInstanceRepository(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task AddAsync(JobInstance jobInstance)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            INSERT INTO job_instance (
                JobId, 
                Status, 
                CreatedTime,  
                CreatedById, 
                Active
            )
            VALUES (
                @JobId, 
                @Status, 
                @CreatedTime, 
                @CreatedById,  
                @Active
            )
            RETURNING Id;";

        jobInstance.Id = await connection.QueryFirstOrDefaultAsync<long>(sql, new
        {
            jobInstance.JobId,
            jobInstance.Status,
            jobInstance.CreatedTime,
            jobInstance.CreatedById,
            jobInstance.Active
        });
    }

    public async Task UpdateAsync(JobInstance jobInstance)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            UPDATE job_instance
            SET JobId = @JobId,
                Status = @Status,
                UpdatedTime = @UpdatedTime,
                UpdatedById = @UpdatedById,
                Active = @Active
            WHERE Id = @Id;";

        await connection.ExecuteAsync(sql, new
        {
            jobInstance.JobId,
            jobInstance.Status,
            jobInstance.UpdatedTime,
            jobInstance.UpdatedById,
            jobInstance.Active,
            jobInstance.Id
        });
    }

    public async Task<JobInstance?> GetByIdAsync(long jobInstanceId, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT Id, JobId, Status, CreatedTime, UpdatedTime, CreatedById, UpdatedById, Active
            FROM job_instance
            WHERE Id = @Id;";

        return await connection.QuerySingleOrDefaultAsync<JobInstance>(sql, new { Id = jobInstanceId });
    }
}
