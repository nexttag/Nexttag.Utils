using Dapper;

namespace Nexttag.Database
{
    public interface IDbParameterBuilder
    {
        string BuildClauses(IEnumerable<IFilter> filters);
        DynamicParameters BuildParameters(IEnumerable<IFilter> filters);
        string BuildOrderClause(string orderBy);
        string BuildPaginationClause(PaginationContext pagination);
        string BuildQuery(string baseQuery, string orderBy, PaginationContext pagination);
    }
}