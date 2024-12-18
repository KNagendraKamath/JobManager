using System.Data;
using Npgsql;

namespace JobScheduler.Infrastructure.Abstractions;

public class PostGresSqlProvider:ISqlProvider
{
    private string _connectionString = "Server=AUTOHPDCSDWBAVD\\SQLEXPRESS;Database=JobScheduling;user id=sa; password=admin1234567890;TrustServerCertificate=True;";

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
