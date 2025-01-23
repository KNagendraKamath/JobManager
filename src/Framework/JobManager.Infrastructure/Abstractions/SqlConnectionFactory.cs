using System.Data;
using JobManager.Framework.Application.Abstractions.Database;
using Npgsql;

namespace JobManager.Framework.Infrastructure.Abstractions;

internal sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString) => 
        _connectionString = connectionString;

    public IDbConnection CreateConnection()
    {
        NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
}
