using System.Data;
using Dapper;
using JobManager.Framework.Application.Abstractions.Database;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Infrastructure.JobSchedulerInstance;

internal sealed class JobStepInstanceRepository : IJobStepInstanceRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JobStepInstanceRepository(ISqlConnectionFactory sqlConnectionFactory) => 
        _sqlConnectionFactory = sqlConnectionFactory;

    public async Task<JobStepInstance?> GetByIdAsync(long Id, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string query = @"
            SELECT 
                id, job_instance_id, job_step_id, status, start_time, end_time, created_time, updated_time, active
            FROM 
                JOB.job_step_instance
            WHERE 
                id = @Id";

        JobStepInstance jobStepInstance = await connection.QuerySingleOrDefaultAsync<JobStepInstance>(query, new { Id = Id });
        return jobStepInstance;
    }

    public async Task<bool> UpdateAsync(JobStepInstance jobStepInstance)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string query = @"
            UPDATE JOB.job_step_instance
            SET 
                job_instance_id = @JobInstanceId,
                job_step_id = @JobStepId,
                status = @Status,
                start_time = @StartTime,
                end_time = @EndTime,
                updated_time = @UpdatedTime,
                active = @Active
            WHERE 
                id = @Id";

        int affectedRows = await connection.ExecuteAsync(query, new
        {
            jobStepInstance.JobInstanceId,
            jobStepInstance.JobStepId,
            jobStepInstance.Status,
            jobStepInstance.StartTime,
            jobStepInstance.EndTime,
            UpdatedTime = DateTime.UtcNow,
            jobStepInstance.Active,
            jobStepInstance.Id
        });

        return affectedRows > 0;
    }
}
