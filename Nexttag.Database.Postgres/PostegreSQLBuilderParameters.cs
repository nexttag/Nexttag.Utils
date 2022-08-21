using System.Dynamic;
using Dapper;

namespace Nexttag.Database.Postgres
{
    public class PostegreSQLBuilderParameters : IDbParameterBuilder
    {
        public string BuildClauses(IEnumerable<IFilter> filters)
        {
            if (filters == null)
                return "";

            var typedFilters = filters.GroupBy(f => f.GetType());
            var clauses = new List<string>(typedFilters.Count());
            foreach (var typedFilter in typedFilters)
            {
                var field = typedFilter.First();
                if (typedFilter.Any(v => v.Value != null && v.Operator == OperatorType.Equal))
                {
                    clauses.Add($"{field.Field} {(typedFilter.Count() > 1 ? " IN " : " = ")} @{field.Variable}");
                }
                else if (typedFilter.Any(v => v.Value != null && v.Operator == OperatorType.NotEqual))
                {
                    clauses.Add($"{field.Field} {(typedFilter.Count() > 1 ? " NOT IN " : " <> ")} @{field.Variable}");
                }
                if (typedFilter.All(v => v.Value != null && v.Operator == OperatorType.Custom))
                {
                    clauses.Add((string)field.Field);
                }
            }
            return @$"{(clauses.Count > 0 ? " WHERE " : "")} {string.Join(" AND ", clauses)} ";
        }

        public string BuildOrderClause(string orderBy)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return string.Empty;
            }
            else
            {
                return $"ORDER BY {orderBy}";
            }
        }

        public string BuildPaginationClause(PaginationContext pagination)
        {
            if (pagination == null || !pagination.Size.HasValue)
            {
                return string.Empty;
            }
            else
            {
                return $"LIMIT {pagination.Size} OFFSET {pagination.Size * (pagination.Page - 1)}";
            }
        }

        public DynamicParameters BuildParameters(IEnumerable<IFilter> filters)
        {
            DynamicParameters parameters = new DynamicParameters();
            if (filters == null)
                return null;

            if (filters != null)
            {
                var typedFilters = filters.GroupBy(f => f.GetType());
                foreach (var typedFilter in typedFilters)
                {
                    var field = typedFilter.First();
                    ExpandoObject parameter = new ExpandoObject();
                    parameter.TryAdd(
                        field.Variable,
                        typedFilter.Count() == 1 ? field.Value : typedFilter.Where(f => f.Value != null).Select(f => f.Value)
                    );
                    parameters.AddDynamicParams(parameter);
                }
            }
            return parameters;
        }

        public string BuildQuery(string baseQuery, string orderBy, PaginationContext pagination)
        {
            var orderByClause = BuildOrderClause(orderBy);
            var paginationClause = BuildPaginationClause(pagination);
            if (pagination == null || string.IsNullOrEmpty(pagination.Query))
            {
                return $"{baseQuery} {orderByClause} {paginationClause};";
            }
            else
            {
                return $"{pagination.Query};{baseQuery} {orderByClause} {paginationClause};";
            }
        }
    }
}