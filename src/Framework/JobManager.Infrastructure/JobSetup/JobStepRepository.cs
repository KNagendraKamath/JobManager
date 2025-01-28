using System.Data;
using Dapper;
using JobManager.Framework.Application.Abstractions.Database;
using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Infrastructure.JobSetup;

internal sealed class JobStepRepository : IJobStepRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JobStepRepository(ISqlConnectionFactory sqlConnectionFactory) 
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<JobStep?> GetJobStep(long jobId, string jobName, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT job_step.*
            FROM JOB.job_step job_step
                join JOB.job_config job_config
                    on job_step.job_config_id = job_config.id
            WHERE job_step.job_Id = @jobId and job_config.name = @jobName";

        return await connection.QuerySingleOrDefaultAsync<JobStep>(sql,
                                                                   new { jobId = jobId, jobName = jobName });
    }
}
