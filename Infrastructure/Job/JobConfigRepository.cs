using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using JobScheduler.Domain.Jobs;
using JobScheduler.Infrastructure.Abstractions;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace JobScheduler.Infrastructure.Job;
public class JobConfigRepository : IJobConfigRepository
{
    private readonly JobDapper _jobDapper;
    public JobConfigRepository(JobDapper jobDapper)
    {
        _jobDapper = jobDapper;
    }
   public IEnumerable<JobConfig> GetAll()
    {
        SqlConnection connection = _jobDapper.GetConnection();
        connection.Open();
        string query = "Select * from JobConfig";
        IEnumerable<JobConfig> result = connection.Query<JobConfig>(query);
        return result;
    }
   public JobConfig GetById(int id)
    {
        SqlConnection connection = _jobDapper.GetConnection();
        connection.Open();
        string query = "SELECT * FROM JobConfig WHERE Id = @Id";
        JobConfig result = connection.QuerySingleOrDefault<JobConfig>(query, new { Id = id });
        return result;
    }
    public void Add(JobConfig jobConfig)
    {
        SqlConnection connection = _jobDapper.GetConnection();
        connection.Open();
        string query = "Insert into JobConfig(Name) VALUES (@Name)";
        connection.Execute(query, new { jobConfig.Name });
    }
    public void Update(JobConfig jobConfig)
    {
         SqlConnection connection = _jobDapper.GetConnection();
         connection.Open();
        string query = "Update jobConfig SET Name = @Name Where Id = {JobConfig.Id}";
        connection.Execute(query, new { jobConfig.Name });
    }
    public void Delete(int id)
    {
        SqlConnection connection = _jobDapper.GetConnection();
        connection.Open();
        string query = "Delete FROM jobConfig WHERE Id = @Id";
        connection.Execute(query, new { Id = id });
    }
}
