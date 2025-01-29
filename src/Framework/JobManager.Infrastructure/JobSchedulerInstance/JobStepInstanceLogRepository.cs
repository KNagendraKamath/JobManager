using System.Data;
using Dapper;
using JobManager.Framework.Application.Abstractions.Database;
using JobManager.Framework.Domain.JobSchedulerInstance;

namespace JobManager.Framework.Infrastructure.JobSchedulerInstance;

internal sealed class JobStepInstanceLogRepository : IJobStepInstanceLogRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JobStepInstanceLogRepository(ISqlConnectionFactory sqlConnectionFactory) =>
        _sqlConnectionFactory = sqlConnectionFactory;

    public async Task AddAsync(JobStepInstanceLog jobStepInstanceLog)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string query = @"
        INSERT INTO JOB.job_step_instance_log (
            job_step_instance_id,
            log,
            created_time,
            active
        ) VALUES (
            @JobStepInstanceId,
            @Log,
            @CreatedTime,
            @Active
        )";

        var parameters = new
        {
            jobStepInstanceLog.JobStepInstanceId,
            jobStepInstanceLog.Log,
            jobStepInstanceLog.CreatedTime,
            jobStepInstanceLog.Active
        };

        await connection.QueryAsync(query, parameters);
    }
}
