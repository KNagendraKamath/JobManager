using System.Data;
using Dapper;
using JobManager.Framework.Application.Abstractions.Database;
using JobManager.Framework.Domain.JobSetup;
using Job = JobManager.Framework.Domain.JobSetup.Job;

namespace JobManager.Framework.Infrastructure.JobSetup;
internal sealed class JobRepository : IJobRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JobRepository(ISqlConnectionFactory sqlConnectionFactory)=> _sqlConnectionFactory = sqlConnectionFactory;

    public async Task<long> AddAsync(Job job)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
        using IDbTransaction transaction = connection.BeginTransaction();

        try
        {
            const string sql = @"
                              INSERT INTO JOB.job(
                                  description, 
                                  effective_date_time, 
                                  type, 
                                  created_time,  
                                  active,
                                  cron_expression
                              )
                              VALUES (
                                  @Description, 
                                  @EffectiveDateTime, 
                                  @Type, 
                                  @CreatedTime, 
                                  @Active,
                                  @CronExpression
                              )
                              RETURNING id;";

            job.Id = await connection.QueryFirstOrDefaultAsync<long>(sql, job);

            const string jobStepSql = @"
                                          INSERT INTO JOB.job_step(
                                              job_id,
                                              job_config_id,
                                              parameter,
                                              created_time,
                                              active
                                          )
                                          VALUES (
                                              @JobId,
                                              @JobConfigId,
                                              @JsonParameter,
                                              @CreatedTime,
                                              @Active
                                          );";
            foreach (JobStep jobStep in job.JobSteps)
            {
                DynamicParameters jobStepParameters = new DynamicParameters(jobStep);
                jobStepParameters.Add("JobId", job.Id);
                await connection.ExecuteAsync(jobStepSql, jobStepParameters);
            }


            if (job.RecurringDetail != null)
            {
                const string recurringSql = @"
                                          INSERT INTO JOB.recurring_detail(
                                              job_id,
                                              recurring_type,
                                              second,
                                              minute,
                                              hour,
                                              day_of_week,
                                              day,
                                              created_time,
                                              active
                                          )
                                          VALUES (
                                              @JobId,
                                              @RecurringType,
                                              @Second,
                                              @Minute,
                                              @Hour,
                                              @DayOfWeek,
                                              @Day,
                                              @CreatedTime,
                                              @Active
                                          );";

                DynamicParameters recurringDetailParameters = new DynamicParameters(job.RecurringDetail);
                recurringDetailParameters.Add("JobId", job.Id);
                await connection.ExecuteAsync(recurringSql, recurringDetailParameters);
            }
            transaction.Commit();
            return job.Id;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task DeactivateJobAsync(long jobId, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
                              UPDATE JOB.job
                              SET 
                                  active = 0,
                                  updated_time = @UpdatedTime
                              WHERE id = @Id;";

        await connection.ExecuteAsync(sql, new
        {
            Id = jobId,
            UpdatedTime = DateTime.UtcNow
        });
    }

    public async Task<Job?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
                         SELECT  job.id ""Id"", 
                                 job.description ""Description"", 
                                 job.effective_date_time ""EffectiveDateTime"", 
                                 job.type ""Type"", 
                                 job.active ""Active"",
                                 recurring_detail.id as ""RecurringDetailId"",
                                 recurring_detail.recurring_type ""RecurringType"", 
                                 recurring_detail.second ""Second"", 
                                 recurring_detail.minute ""Minute"", 
                                 recurring_detail.hour ""Hour"", 
                                 recurring_detail.day_of_week ""DayOfWeek"", 
                                 recurring_detail.day ""Day"",
                                 job_step.id as ""JobStepId"",
                                 job_step.job_config_id as ""JobConfigId"",
                                 job_step.parameter as ""JsonParameter"",
                                 job_step.created_time as ""CreatedTime"",
                                 job_step.active as ""StepActive""
                          FROM JOB.job job
                          LEFT JOIN JOB.recurring_detail recurring_detail
                              ON job.id = recurring_detail.job_id
                          LEFT JOIN JOB.job_step job_step
                              ON job.id = job_step.job_id
                          WHERE job.id = @Id;";

        Dictionary<long, Job> jobDictionary = new Dictionary<long, Job>();

        IEnumerable<Job> result = await connection.QueryAsync<Job, RecurringDetail, JobStep, Job>(
            sql,
            (job, recurringDetail, jobStep) =>
            {
                if (!jobDictionary.TryGetValue(job.Id, out Job? currentJob))
                {
                    currentJob = job;
                    jobDictionary.Add(currentJob.Id, currentJob);
                }

                if (recurringDetail is not null)
                    currentJob.SetRecurringDetail(recurringDetail);

                if (jobStep is not null)
                    currentJob.AddJobStep(jobStep);

                return currentJob;
            },
            new { Id = id },
            splitOn: "RecurringDetailId,JobStepId"
        );

        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<Job>> GetJobs(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
                              SELECT job.id ""Id"", 
                                     job.description ""Description"", 
                                     job.effective_date_time ""EffectiveDateTime"", 
                                     job.type ""Type"", 
                                     job.cron_expression ""CronExpression"",
                                     job.active ""Active"",
                                     recurring_detail.id As ""RecurringDetailId"",
                                     recurring_detail.recurring_type ""RecurringType"", 
                                     recurring_detail.second ""Second"", 
                                     recurring_detail.minute ""Minute"", 
                                     recurring_detail.hour ""Hour"", 
                                     recurring_detail.day_of_week ""DayOfWeek"", 
                                     recurring_detail.day ""Day""
                              FROM JOB.job job
                                  LEFT JOIN JOB.recurring_detail recurring_detail
                                      ON job.id = recurring_detail.job_id
                              ORDER BY job.id
                              OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        IEnumerable<Job> jobs= await connection.QueryAsync<Job, RecurringDetail, Job>(
                                                 sql,
                                                  (job, recurringDetail) =>
                                                  {
                                                      if (recurringDetail != null) job.SetRecurringDetail(recurringDetail);

                                                      return job;
                                                  },
                                                 new
                                                 {
                                                     Offset = (page - 1) * pageSize,
                                                     PageSize = pageSize
                                                 },
                                                 splitOn: "RecurringDetailId"
                                                );
        if (jobs.Any())
        {
            const string jobStepSql = @"
            SELECT job_step.id as ""JobStepId"",
                   job_step.job_id as ""JobId"",
                   job_step.job_config_id as ""JobConfigId"",
                   job_step.parameter as ""JsonParameter"",
                   job_step.created_time as ""CreatedTime"",
                   job_step.active as ""StepActive""
            FROM JOB.job_step job_step
            WHERE job_step.job_id = ANY(@JobIds);";

            IEnumerable<JobStep> jobSteps = await connection.QueryAsync<JobStep>(
                jobStepSql,
                new { JobIds = jobs.Select(x=>x.Id).ToArray() }
                );

            foreach(Job job in  jobs)
               job.JobSteps.AddRange(jobSteps.Where(x => x.JobId == job.Id)); 
        }

        return jobs;
    }

    public async Task RemoveJobStep(long jobStepId, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
                              UPDATE JOB.job_step
                              SET 
                                  active = 0,
                                  updated_time = @UpdatedTime
                              WHERE id = @Id;";

        await connection.ExecuteAsync(sql, new
        {
            Id = jobStepId,
            UpdatedTime = DateTime.UtcNow
        });
    }
}
