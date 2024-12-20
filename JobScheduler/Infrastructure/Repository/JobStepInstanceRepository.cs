﻿using System.Data;
using Dapper;
using JobScheduler.Domain.Models;
using JobScheduler.Infrastructure.Abstractions;

namespace JobScheduler.Infrastructure.Repository;

public class JobStepInstanceRepository:IJobStepInstanceRepository
{
    private readonly ISqlProvider _sqlProvider;

    public JobStepInstanceRepository(ISqlProvider sqlProvider)
    {
        _sqlProvider = sqlProvider;
    }

    public async Task<long> AddAsync(JobStepInstance jobStepInstance)
    {
        const string sql = @"
            INSERT INTO JobStepInstance (
                JobInstanceId, 
                JobStatus, 
                StartTime, 
                EndTime, 
                Active, 
                CreatedTime, 
                UpdatedTime, 
                CreatedById, 
                UpdatedById)
            VALUES (@JobInstanceId, 
                    @JobStatus, 
                    @StartTime, 
                    @EndTime, 
                    @Active, 
                    @CreatedTime, 
                    @UpdatedTime, 
                    @CreatedById, 
                    @UpdatedById)
            RETURNING Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.ExecuteScalarAsync<long>(sql, jobStepInstance);
    }

    public async Task<JobStepInstance> GetByIdAsync(long id)
    {
        const string sql = "SELECT * FROM JobStepInstance WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<JobStepInstance>(sql, new { Id = id });
    }

    public async Task<IEnumerable<JobStepInstance>> GetAllAsync()
    {
        const string sql = "SELECT * FROM JobStepInstance;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        return await connection.QueryAsync<JobStepInstance>(sql);
    }

    public async Task<bool> UpdateAsync(JobStepInstance jobStepInstance)
    {
        const string sql = @"
            UPDATE JobStepInstance
            SET JobInstanceId = @JobInstanceId,
                JobStatus = @JobStatus,
                StartTime = @StartTime,
                EndTime = @EndTime,
                Active = @Active,
                CreatedTime = @CreatedTime,
                UpdatedTime = @UpdatedTime,
                CreatedById = @CreatedById,
                UpdatedById = @UpdatedById
            WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        int rowsAffected = await connection.ExecuteAsync(sql, jobStepInstance);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        const string sql = "DELETE FROM JobStepInstance WHERE Id = @Id;";
        using IDbConnection connection = _sqlProvider.CreateConnection();
        int rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }
}
