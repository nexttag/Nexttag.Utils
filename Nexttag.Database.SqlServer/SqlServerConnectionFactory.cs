using System.Data;
using Microsoft.Data.SqlClient;

namespace Nexttag.Database.SqlServer;

public class SqlServerConnectionFactory : IConnectionFactory
{
    private readonly string _connectionString;

    public SqlServerConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}