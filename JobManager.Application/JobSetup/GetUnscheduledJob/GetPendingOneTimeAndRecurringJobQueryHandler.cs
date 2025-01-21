using System.Data;
using Dapper;
using JobManager.Framework.Application.Abstractions.Database;
using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.Abstractions;

namespace JobManager.Framework.Application.JobSetup.GetJobDetail;

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
            	 recurring_detail.id As RecurringDetailId,
            	 recurring_detail.recurring_type As RecurringType,
                 recurring_detail.second As Second,
                 recurring_detail.minute As Minute,
                 recurring_detail.hours As Hour,
                 recurring_detail.day_of_week As DayOfWeek,
                 recurring_detail.day As Day,
                 job_step.id AS JobStepId,
                 job_config.name AS JobConfigName,
                 job_config.Assembly AS Assembly,
                 job_step.json_parameter As JsonParameter
             FROM job
                 JOIN job_step
                     ON job.id = job_step.job_id 
                        and job.Active = true 
                        and job_step.Active = true
                 JOIN job_config
                     ON job_step.job_config_id = job_config.id
            	 LEFT JOIN recurring_detail
            	 	 ON recurring_detail.job_id = job.id
                 LEFT JOIN LATERAL 
                     (
                    	SELECT unnest(string_to_array(@AlreadyScheduledJobIdsInCsv, ','))::BIGINT AS Id
                     ) AS jobs_scheduled ON job.id = jobs_scheduled.Id
                LEFT JOIN job_Instance
            	    on job.id = job_instance.job_id 
                    and job.type = 'Onetime'
             WHERE jobs_scheduled.Id IS NULL and job_instance.Id is null;
            """;

        
         return (await connection.QueryAsync<JobResponse, JobStepResponse, JobResponse>
                        (
                           sql,
                           (job, step) =>
                              {
                                  job.Steps.Add(step);
                                  return job;
                              },
                           new { request.AlreadyScheduledJobIdsInCsv },
                           splitOn: "JobStepId"
                        )
                ).ToList();

    }
}
