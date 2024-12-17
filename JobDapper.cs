using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace JobScheduler;

public class JobDapper
{
    private string _connection = "Server=AUTOHPDCSDWBAVD\\SQLEXPRESS;Database=JobScheduling;user id=sa; password=admin1234567890;TrustServerCertificate=True;";

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connection);
    }
}
