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
                Job.Id as JobId,
                Job.Description,
                Job.EffectiveDateTime,
                JobStep.Id as JobStepId,
                JobConfig.Name as JobConfigName,
                JobStep.JsonParameter
            FROM Job
                JOIN JobStep
                    ON Job.Id = JobStep.JobId
                JOIN JobConfig
                    ON JobStep.JobConfigId = JobConfig.Id
            WHERE Job.Type = 'Onetime'
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
