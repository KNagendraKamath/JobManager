using System.Data;
using JobManager.Framework.Application.Abstractions.Database;
using JobManager.Framework.Application.JobSetup;
using Dapper;
using JobManager.Framework.Application.JobSetup.ScheduleJob;
using JobManager.Framework.Application.JobSetup.UnscheduleJob;

namespace JobManager.Framework.Infrastructure.JobSetup;
internal sealed class JobQuery : IJobQuery
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JobQuery(ISqlConnectionFactory sqlConnectionFactory) =>
        _sqlConnectionFactory = sqlConnectionFactory;

    public async Task<IReadOnlyList<JobGroups>> GetJobsToUnschedule(long[] alreadyScheduledJobIds, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
             SELECT
                 job.id AS ""JobId"",
                 job_step.id As ""JobStepId""
             FROM JOB.job job
                 JOIN JOB.job_step job_step
                     ON job.id = job_step.job_id 
                 JOIN JOB.job_config job_config
                     ON job_step.job_config_id = job_config.id
             WHERE job.id = ANY(@AlreadyScheduledJobIds) AND (job.Active = false or job_step.Active = false)
            ";

        return (await connection.QueryAsync<JobGroups>
                       (
                          sql,
                          new { AlreadyScheduledJobIds=alreadyScheduledJobIds }
                       )
               ).ToList();
    }

    public async Task<IReadOnlyList<JobResponse>> GetPendingOneTimeAndRecurringActiveJobs(long[] alreadyScheduledJobIds, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
             SELECT
                 job.id AS ""JobId"",
                 job.description AS ""Description"",
                 job.effective_date_time AS ""EffectiveDateTime"",
                 job.type AS ""JobType"",
                 recurring_detail.id AS ""RecurringDetailId"",
                 recurring_detail.recurring_type AS ""RecurringType"",
                 recurring_detail.second AS ""Second"",
                 recurring_detail.minute AS ""Minute"",
                 recurring_detail.hour AS ""Hour"",
                 recurring_detail.day_of_week AS ""DayOfWeek"",
                 recurring_detail.day AS ""Day"",
                 job_step.id AS ""JobStepId"",
                 job_config.name AS ""JobConfigName"",
                 job_step.parameter AS ""JsonParameter""
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
                    	SELECT unnest(@AlreadyScheduledJobIds) AS Id
                     ) AS jobs_scheduled ON job.id = jobs_scheduled.Id
                LEFT JOIN JOB.job_Instance job_instance
            	    on job.id = job_instance.job_id 
                    and job.type = 'Onetime'
             WHERE jobs_scheduled.Id IS NULL and job_instance.Id is null;
            ";


        return (await connection.QueryAsync<JobResponse, JobStepResponse, JobResponse>
                       (
                          sql,
                          (job, step) =>
                          {
                              job.Steps.Add(step);
                              return job;
                          },
                          new { AlreadyScheduledJobIds=alreadyScheduledJobIds },
                          splitOn: "JobStepId"
                       )
               ).ToList();
    }
}
