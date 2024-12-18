using System.Data;

namespace JobScheduler.Infrastructure.Abstractions;

public interface ISqlProvider
{
    IDbConnection CreateConnection();
}
