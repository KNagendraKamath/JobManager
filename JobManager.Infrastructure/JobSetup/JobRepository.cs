using System.Data;
using Dapper;
using JobManager.Framework.Application.Abstractions.Database;
using JobManager.Framework.Domain.JobSetup;
using Job = JobManager.Framework.Domain.JobSetup.Job;

namespace JobManager.Framework.Infrastructure.JobSetup;
internal sealed class JobRepository : IJobRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JobRepository(ISqlConnectionFactory sqlConnectionFactory) => 
        _sqlConnectionFactory = sqlConnectionFactory;

    public async Task<long> AddAsync(Job job)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            INSERT INTO job(
                description, 
                effective_date_time, 
                type, 
                created_time,  
                created_by_id, 
                active
            )
            VALUES (
                @Description, 
                @EffectiveDateTime, 
                @Type, 
                @CreatedTime, 
                @CreatedById,  
                @Active
            )
            RETURNING id;";

        job.Id = await connection.QueryFirstOrDefaultAsync<long>(sql, new
        {
            job.Description,
            job.EffectiveDateTime,
            job.Type,
            job.CreatedTime,
            job.CreatedById,
            job.Active
        });

        if (job.RecurringDetail != null)
        {
            const string recurringSql = @"
                INSERT INTO recurring_detail(
                    job_id,
                    recurring_type,
                    second,
                    minute,
                    hours,
                    day_of_week,
                    day,
                    created_time,
                    created_by_id,
                    active
                )
                VALUES (
                    @JobId,
                    @RecurringType,
                    @Second,
                    @Minute,
                    @Hours,
                    @DayOfWeek,
                    @Day,
                    @CreatedTime,
                    @CreatedById,
                    @Active
                );";

            await connection.ExecuteAsync(recurringSql, new
            {
                JobId = job.Id,
                job.RecurringDetail.RecurringType,
                job.RecurringDetail.Second,
                job.RecurringDetail.Minute,
                job.RecurringDetail.Hours,
                job.RecurringDetail.DayOfWeek,
                job.RecurringDetail.Day,
                job.CreatedTime,
                job.CreatedById,
                job.Active
            });
        }

        return job.Id;
    }

    public async Task DeactivateJobAsync(long jobId, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            UPDATE job
            SET 
                active = 0,
                updated_time = @UpdatedTime,
                updated_by_id = @UpdatedById
            WHERE id = @Id;";

        await connection.ExecuteAsync(sql, new
        {
            Id = jobId,
            UpdatedTime = DateTimeOffset.UtcNow,
            UpdatedById = 1 // Assuming a default user ID for the update
        });
    }

    public async Task<Job?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
           SELECT job.id, 
                   description, 
                   effective_date_time, 
                   type, 
                   active,
                   recurring_detail.id as RecurringDetailId,
                   recurring_type, 
                   second, 
                   minute, 
                   hours, 
                   day_of_week, 
                   day
            FROM job
            LEFT JOIN recurring_detail
                ON job.id = recurring_detail.job_id
            WHERE job.id = @Id;";

        IEnumerable<Job> result = await connection.QueryAsync<Job, RecurringDetail, Job>(
                                                    sql,
                                                    (job, recurringDetail) =>
                                                    {
                                                        if (recurringDetail != null)
                                                            job.SetRecurringDetail(recurringDetail);

                                                        return job;
                                                    },
                                                    new
                                                    {
                                                        Id= id
                                                    },
                                                    splitOn: "RecurringDetailId"
                                );
        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<Job>> GetJobs(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT job.id, 
                   description, 
                   effective_date_time, 
                   type, 
                   active,
                   recurring_detail.id as RecurringDetailId,
                   recurring_type, 
                   second, 
                   minute, 
                   hours, 
                   day_of_week, 
                   day
            FROM job
            LEFT JOIN recurring_detail
                ON job.id = recurring_detail.job_id
            ORDER BY job.id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        return await connection.QueryAsync<Job,RecurringDetail,Job>(
                                                    sql,
                                                    (job, recurringDetail) =>
                                                    {
                                                        if (recurringDetail != null)
                                                            job.SetRecurringDetail(recurringDetail);

                                                        return job;
                                                    },
                                                    new
                                                    {
                                                        Offset = (page - 1) * pageSize,
                                                        PageSize = pageSize
                                                    },
                                                    splitOn: "RecurringDetailId"
                                );
    }

    public async Task RemoveJobStep(long jobStepId, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            UPDATE job_step
            SET 
                active = 0,
                updated_time = @UpdatedTime,
                updated_by_id = @UpdatedById
            WHERE id = @Id;";

        await connection.ExecuteAsync(sql, new
        {
            Id = jobStepId,
            UpdatedTime = DateTimeOffset.UtcNow,
            UpdatedById = 1 // Assuming a default user ID for the update
        });
    }
}
