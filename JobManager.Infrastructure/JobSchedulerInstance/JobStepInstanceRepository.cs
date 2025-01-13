using System.Data;
using Dapper;
using JobManager.Application.Abstractions.Database;
using JobManager.Domain.JobSchedulerInstance;

namespace JobManager.Infrastructure.JobSchedulerInstance;

internal sealed class JobStepInstanceRepository : IJobStepInstanceRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JobStepInstanceRepository(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<JobStepInstance?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string query = @"
            SELECT 
                Id, JobInstanceId, JobStepId, Status, StartTime, EndTime, CreatedTime, UpdatedTime, CreatedById, UpdatedById, Active
            FROM 
                JobStepInstances
            WHERE 
                Id = @Id";

        JobStepInstance jobStepInstance = await connection.QuerySingleOrDefaultAsync<JobStepInstance>(query, new { Id = id });
        return jobStepInstance;

    }

    public async Task<bool> UpdateAsync(JobStepInstance jobStepInstance)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string query = @"
            UPDATE JobStepInstances
            SET 
                JobInstanceId = @JobInstanceId,
                JobStepId = @JobStepId,
                Status = @Status,
                StartTime = @StartTime,
                EndTime = @EndTime,
                UpdatedTime = @UpdatedTime,
                UpdatedById = @UpdatedById,
                Active = @Active
            WHERE 
                Id = @Id";

            int affectedRows = await connection.ExecuteAsync(query, new
            {
                jobStepInstance.JobInstanceId,
                jobStepInstance.JobStepId,
                jobStepInstance.Status,
                jobStepInstance.StartTime,
                jobStepInstance.EndTime,
                UpdatedTime = DateTimeOffset.UtcNow,
                jobStepInstance.UpdatedById,
                jobStepInstance.Active,
                jobStepInstance.Id
            });

            return affectedRows > 0;
    }
}
