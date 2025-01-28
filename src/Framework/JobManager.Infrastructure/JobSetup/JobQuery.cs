using System.Data;
using JobManager.Framework.Application.Abstractions.Database;
using JobManager.Framework.Application.JobSetup;
using JobManager.Framework.Application.JobSetup.GetJobDetail;
using Dapper;

namespace JobManager.Framework.Infrastructure.JobSetup;
internal sealed class JobQuery : IJobQuery
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JobQuery(ISqlConnectionFactory sqlConnectionFactory) =>
        _sqlConnectionFactory = sqlConnectionFactory;

    public async Task<IReadOnlyList<JobResponse>> GetPendingOneTimeAndRecurringJobs(string alreadyScheduledJobIdsInCsv, CancellationToken cancellationToken = default)
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
                 job_step.parameter As JsonParameter
             FROM JOB.job job
                 JOIN JOB.job_step job_step
                     ON job.id = job_step.job_id 
                        and job.Active = true 
                        and job_step.Active = true
                 JOIN JOB.job_config job_config
                     ON job_step.job_config_id = job_config.id
            	 LEFT JOIN JOB.recurring_detail recurring_detail
            	 	 ON recurring_detail.job_id = job.id
                 LEFT JOIN LATERAL 
                     (
                    	SELECT unnest(string_to_array(@AlreadyScheduledJobIdsInCsv, ','))::BIGINT AS Id
                     ) AS jobs_scheduled ON job.id = jobs_scheduled.Id
                LEFT JOIN JOB.job_Instance job_instance
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
                          new { alreadyScheduledJobIdsInCsv },
                          splitOn: "JobStepId"
                       )
               ).ToList();
    }
}
