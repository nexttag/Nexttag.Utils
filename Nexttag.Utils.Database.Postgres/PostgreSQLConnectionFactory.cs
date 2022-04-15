using System.Data;
using Npgsql;

namespace Nexttag.Utils.Database.Postgres
{
    public class PostgreSQLConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        public PostgreSQLConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}