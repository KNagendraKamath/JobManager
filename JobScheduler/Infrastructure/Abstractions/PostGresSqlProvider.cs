using System.Data;
using Npgsql;

namespace JobScheduler.Infrastructure.Abstractions;

public class PostGresSqlProvider:ISqlProvider
{
    private string _connectionString = "Host=127.0.0.1;Database=JobManager;Username=postgres;Password=admin;Port=5432;SSL Mode=Disable";

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
