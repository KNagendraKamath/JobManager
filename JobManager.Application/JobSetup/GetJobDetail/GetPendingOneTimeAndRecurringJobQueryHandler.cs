using System.Data;
using JobManager.Application.Abstractions.Database;
using JobManager.Application.Abstractions.Messaging;
using JobManager.Domain.Abstractions;
using Dapper;

namespace JobManager.Application.JobSetup.GetJobDetail;

internal sealed class GetPendingOneTimeAndRecurringJobQueryHandler : IQueryHandler<GetPendingOneTimeAndRecurringJobQuery, List<JobResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetPendingOneTimeAndRecurringJobQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<List<JobResponse>>> Handle(GetPendingOneTimeAndRecurringJobQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT
                job.id AS JobId,
                job.description as Description,
                job.effective_date_time as EffectiveDateTime,
                job_step.id AS JobStepId,
                job_config.name AS JobConfigName,
                job_step.json_parameter As JsonParameter
            FROM job
                JOIN job_step
                    ON job.id = job_step.job_id
                JOIN job_config
                    ON job_step.job_config_id = job_config.id
            WHERE job.type = 'Onetime';
            """;

        
         return (await connection.QueryAsync<JobResponse, JobStepResponse, JobResponse>
                        (
                           sql,
                           (job, jobStep) =>
                              {
                                  job.Steps.Add(jobStep);
                                  return job;
                              },
                           splitOn: "JobStepId"
                        )
                ).ToList();

    }
}
