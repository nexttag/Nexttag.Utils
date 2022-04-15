using System.Data;

namespace Nexttag.Utils.Database
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
    }
}