using System.Data;
using Dapper;
using JobManager.Framework.Application.Abstractions.Database;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Infrastructure.JobSchedulerInstance;

internal sealed class JobInstanceRepository : IJobInstanceRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JobInstanceRepository(ISqlConnectionFactory sqlConnectionFactory) =>
        _sqlConnectionFactory = sqlConnectionFactory;

    public async Task AddAsync(JobInstance jobInstance)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            INSERT INTO job_instance (
                job_id, 
                status, 
                created_time,  
                created_by_id, 
                active
            )
            VALUES (
                @JobId, 
                @Status, 
                @CreatedTime, 
                @CreatedById,  
                @Active
            )
            RETURNING id;";

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
            SET job_id = @JobId,
                status = @Status,
                updated_time = @UpdatedTime,
                updated_by_id = @UpdatedById,
                active = @Active
            WHERE id = @Id;";

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

    public async Task<JobInstance?> GetByIdAsync(long jobInstanceId, CancellationToken cancellationToken=default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT id, job_id, status, created_time, updated_time, created_by_id, updated_by_id, active
            FROM job_instance
            WHERE id = @Id;";

        return await connection.QuerySingleOrDefaultAsync<JobInstance>(sql, new { Id = jobInstanceId });
    }
}
