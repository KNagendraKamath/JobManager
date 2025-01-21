using System.Data;

namespace JobManager.Framework.Application.Abstractions.Database;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}