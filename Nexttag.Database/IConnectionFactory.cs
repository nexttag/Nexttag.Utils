using System.Data;

namespace Nexttag.Database
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
    }
}