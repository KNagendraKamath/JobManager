using System.Data;

namespace JobManager.Application.Abstractions.Database;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}