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
                 job.type as JobType,
                 job_step.id AS JobStepId,
                 job_config.name AS JobConfigName,
                 job_step.json_parameter As JsonParameter,
            	 recurring_detail.id As RecurringDetailId,
            	 recurring_detail.recurring_type As RecurringType,
                 recurring_detail.second As Seconds,
                 recurring_detail.minutes As Minutes,
                 recurring_detail.hours As Hours,
                 recurring_detail.day_of_week As DayOfWeek,
                 recurring_detail.day As Day
             FROM job
                 JOIN job_step
                     ON job.id = job_step.job_id and job.Active = true
                 JOIN job_config
                     ON job_step.job_config_id = job_config.id
            	 LEFT JOIN recurring_detail
            	 	 ON recurring_detail.job_id = job.id
                 LEFT JOIN LATERAL 
                     (
                    	SELECT unnest(string_to_array(@ScheduledJobIdsInCsv, ','))::BIGINT AS Id
                     ) AS jobs_scheduled ON job.id = jobs_scheduled.Id
             WHERE jobs_scheduled.Id IS NULL;
            """;

        
         return (await connection.QueryAsync<JobResponse, RecurringDetailResponse, JobResponse>
                        (
                           sql,
                           (job, recurringDetail) =>
                              {
                                  job.SetRecurringDetail(recurringDetail);
                                  return job;
                              },
                           new { request.ScheduledJobIdsInCsv },
                           splitOn: "RecurringDetailId"
                        )
                ).ToList();

    }
}
