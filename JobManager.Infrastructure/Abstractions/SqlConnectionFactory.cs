using System.Data;
using JobManager.Application.Abstractions.Database;
using Npgsql;

namespace JobManager.Infrastructure.Abstractions;

internal sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
}
