using Dapper;
using Dommel;
using Microsoft.Extensions.Logging;

namespace Nexttag.Database
{
    public class Repository<T> : DatabaseContextContainer, IRepository<T> where T : class
    {
        private readonly DatabaseContext _dbContext;
        private readonly ILogger<Repository<T>> _logger;
        private static PaginationContext ONE_RESULT = new PaginationContext(1, 1, string.Empty);

        public Repository(DatabaseContext dbContext, ILogger<Repository<T>> logger) : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public string Insert(T entity)
        {
            var id = _dbContext.Connection.Insert(entity, _dbContext.Transaction).ToString();

            return id;
        }

        public T Update(T entity)
        {
            if (!_dbContext.Connection.Update(entity, _dbContext.Transaction))
                throw new Exception("Update not happen");

            return entity;
        }

        public bool Delete(string id)
        {
            var entity = _dbContext.Connection.Get<T>(id);
            return _dbContext.Connection.Delete(entity, _dbContext.Transaction);
        }

        public IEnumerable<T> Query(string baseQuery, IEnumerable<IFilter> filters, string orderBy = "", PaginationContext pagination = null)
        {
            DynamicParameters parameters = _dbContext.BuildParameters(filters);
            var where = _dbContext.BuildClauses(filters);
            if (pagination != null && !string.IsNullOrEmpty(pagination.Query))
            {
                pagination.Query = $"{pagination.Query} {where};";
            }
            var results = Query($"{baseQuery} {where}", parameters, orderBy, pagination);
            return results;
        }

        public IEnumerable<T> Query<T1>(string baseQuery, Func<T, T1, T> transformationFunction, IEnumerable<IFilter> filters, string splitOn = "Id, Id", string orderBy = "", PaginationContext pagination = null)
        {
            DynamicParameters parameters = _dbContext.BuildParameters(filters);
            var where = _dbContext.BuildClauses(filters);
            if (pagination != null && !string.IsNullOrEmpty(pagination.Query))
            {
                pagination.Query = $"{pagination.Query} {where};";
            }
            var results = Query($"{baseQuery} {where}", transformationFunction, parameters, splitOn, orderBy, pagination);
            return results;
        }

        public IEnumerable<T> Query<T1, T2>(string baseQuery, Func<T, T1, T2, T> transformationFunction, IEnumerable<IFilter> filters, string splitOn = "Id, Id, Id", string orderBy = "", PaginationContext pagination = null)
        {
            DynamicParameters parameters = _dbContext.BuildParameters(filters);
            var where = _dbContext.BuildClauses(filters);
            if (pagination != null && !string.IsNullOrEmpty(pagination.Query))
            {
                pagination.Query = $"{pagination.Query} {where};";
            }
            var results = Query($"{baseQuery} {where}", transformationFunction, parameters, splitOn, orderBy, pagination);
            return results;
        }

        public IEnumerable<T> Query<T1, T2, T3>(string baseQuery, Func<T, T1, T2, T3, T> transformationFunction, IEnumerable<IFilter> filters, string splitOn = "Id, Id, Id, Id", string orderBy = "", PaginationContext pagination = null)
        {
            DynamicParameters parameters = _dbContext.BuildParameters(filters);
            var where = _dbContext.BuildClauses(filters);
            if (pagination != null && !string.IsNullOrEmpty(pagination.Query))
            {
                pagination.Query = $"{pagination.Query} {where};";
            }
            var results = Query($"{baseQuery} {where}", transformationFunction, parameters, splitOn, orderBy, pagination);
            return results;
        }

        public IEnumerable<T> Query<T1, T2, T3, T4>(string baseQuery, Func<T, T1, T2, T3, T4, T> transformationFunction, IEnumerable<IFilter> filters, string splitOn = "Id, Id, Id, Id, Id", string orderBy = "", PaginationContext pagination = null)
        {
            DynamicParameters parameters = _dbContext.BuildParameters(filters);
            var where = _dbContext.BuildClauses(filters);
            if (pagination != null && !string.IsNullOrEmpty(pagination.Query))
            {
                pagination.Query = $"{pagination.Query} {where};";
            }
            var results = Query($"{baseQuery} {where}", transformationFunction, parameters, splitOn, orderBy, pagination);
            return results;
        }

        public IEnumerable<T> Query<T1, T2, T3, T4, T5>(string baseQuery, Func<T, T1, T2, T3, T4, T5, T> transformationFunction, IEnumerable<IFilter> filters, string splitOn = "Id, Id, Id, Id, Id, Id", string orderBy = "", PaginationContext pagination = null)
        {
            DynamicParameters parameters = _dbContext.BuildParameters(filters);
            var where = _dbContext.BuildClauses(filters);
            if (pagination != null && !string.IsNullOrEmpty(pagination.Query))
            {
                pagination.Query = $"{pagination.Query} {where};";
            }
            var results = Query($"{baseQuery} {where}", transformationFunction, parameters, splitOn, orderBy, pagination);
            return results;
        }

        public IEnumerable<T> Query(string baseQuery, DynamicParameters parameters, string orderBy = "", PaginationContext pagination = null)
        {
            string query = _dbContext.BuildQuery(baseQuery, orderBy, pagination);
            var results = default(IEnumerable<T>);
            _logger.LogDebug($"[Edelivery.Database] Compiled Query: {query}");
            if (pagination == null || string.IsNullOrEmpty(pagination.Query))
            {
                results = _dbContext.Connection.Query<T>(
                    $"{query};",
                    parameters
                );
            }
            else
            {
                using (var multquery = _dbContext.Connection.QueryMultiple(query, parameters))
                {
                    pagination.Total = multquery.Read<uint>().Single();
                    results = multquery.Read<T>();
                }
            }

            return results;
        }

        public IEnumerable<T> Query<T1>(string baseQuery, Func<T, T1, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id", string orderBy = "", PaginationContext pagination = null)
        {
            string query = _dbContext.BuildQuery(baseQuery, orderBy, pagination);
            var results = default(IEnumerable<T>);
            if (pagination == null || string.IsNullOrEmpty(pagination.Query))
            {
                results = _dbContext.Connection.Query(
                    $"{query};",
                    transformationFunction,
                    parameters,
                    splitOn: splitOn
                );
            }
            else
            {
                using (var multquery = _dbContext.Connection.QueryMultiple(query, parameters))
                {
                    pagination.Total = multquery.Read<uint>().Single();
                    results = multquery.Read(transformationFunction, splitOn: splitOn);
                }
            }

            return results;
        }

        public IEnumerable<T> Query<T1, T2>(string baseQuery, Func<T, T1, T2, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id", string orderBy = "", PaginationContext pagination = null)
        {
            string query = _dbContext.BuildQuery(baseQuery, orderBy, pagination);
            var results = default(IEnumerable<T>);
            if (pagination == null || string.IsNullOrEmpty(pagination.Query))
            {
                results = _dbContext.Connection.Query(
                    $"{query};",
                    transformationFunction,
                    parameters,
                    splitOn: splitOn
                );
            }
            else
            {
                using (var multquery = _dbContext.Connection.QueryMultiple(query, parameters))
                {
                    pagination.Total = multquery.Read<uint>().Single();
                    results = multquery.Read(transformationFunction, splitOn: splitOn);
                }
            }

            return results;
        }

        public IEnumerable<T> Query<T1, T2, T3>(string baseQuery, Func<T, T1, T2, T3, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id, Id", string orderBy = "", PaginationContext pagination = null)
        {
            string query = _dbContext.BuildQuery(baseQuery, orderBy, pagination);
            var results = default(IEnumerable<T>);
            if (pagination == null || string.IsNullOrEmpty(pagination.Query))
            {
                results = _dbContext.Connection.Query(
                    $"{query};",
                    transformationFunction,
                    parameters,
                    splitOn: splitOn
                );
            }
            else
            {
                using (var multquery = _dbContext.Connection.QueryMultiple(query, parameters))
                {
                    pagination.Total = multquery.Read<uint>().Single();
                    results = multquery.Read(transformationFunction, splitOn: splitOn);
                }
            }

            return results;
        }

        public IEnumerable<T> Query<T1, T2, T3, T4>(string baseQuery, Func<T, T1, T2, T3, T4, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id, Id, Id", string orderBy = "", PaginationContext pagination = null)
        {
            string query = _dbContext.BuildQuery(baseQuery, orderBy, pagination);
            var results = default(IEnumerable<T>);
            if (pagination == null || string.IsNullOrEmpty(pagination.Query))
            {
                results = _dbContext.Connection.Query(
                    $"{query};",
                    transformationFunction,
                    parameters,
                    splitOn: splitOn
                );
            }
            else
            {
                using (var multquery = _dbContext.Connection.QueryMultiple(query, parameters))
                {
                    pagination.Total = multquery.Read<uint>().Single();
                    results = multquery.Read(transformationFunction, splitOn: splitOn);
                }
            }

            return results;
        }

        public IEnumerable<T> Query<T1, T2, T3, T4, T5>(string baseQuery, Func<T, T1, T2, T3, T4, T5, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id, Id, Id, Id", string orderBy = "", PaginationContext pagination = null)
        {
            string query = _dbContext.BuildQuery(baseQuery, orderBy, pagination);
            var results = default(IEnumerable<T>);
            if (pagination == null || string.IsNullOrEmpty(pagination.Query))
            {
                results = _dbContext.Connection.Query(
                    $"{query};",
                    transformationFunction,
                    parameters,
                    splitOn: splitOn
                );
            }
            else
            {
                using (var multquery = _dbContext.Connection.QueryMultiple(query, parameters))
                {
                    pagination.Total = multquery.Read<uint>().Single();
                    results = multquery.Read(transformationFunction, splitOn: splitOn);
                }
            }

            return results;
        }

        public T QueryFirst(string baseQuery, IEnumerable<IFilter> filters, string orderBy = "")
        {

            return Query(baseQuery, filters, orderBy, ONE_RESULT).FirstOrDefault();
        }

        public T QueryFirst<T1>(string baseQuery, Func<T, T1, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id", string orderBy = "")
        {
            return Query(baseQuery, transformationFunction, internalFilters, splitOn, orderBy, ONE_RESULT).FirstOrDefault();
        }

        public T QueryFirst<T1, T2>(string baseQuery, Func<T, T1, T2, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id, Id", string orderBy = "")
        {
            return Query(baseQuery, transformationFunction, internalFilters, splitOn, orderBy, ONE_RESULT).FirstOrDefault();
        }

        public T QueryFirst<T1, T2, T3>(string baseQuery, Func<T, T1, T2, T3, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id, Id, Id", string orderBy = "")
        {
            return Query(baseQuery, transformationFunction, internalFilters, splitOn, orderBy, ONE_RESULT).FirstOrDefault();
        }

        public T QueryFirst<T1, T2, T3, T4>(string baseQuery, Func<T, T1, T2, T3, T4, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id, Id, Id, Id", string orderBy = "")
        {
            return Query(baseQuery, transformationFunction, internalFilters, splitOn, orderBy, ONE_RESULT).FirstOrDefault();
        }

        public T QueryFirst<T1, T2, T3, T4, T5>(string baseQuery, Func<T, T1, T2, T3, T4, T5, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id, Id, Id, Id, Id", string orderBy = "")
        {
            return Query(baseQuery, transformationFunction, internalFilters, splitOn, orderBy, ONE_RESULT).FirstOrDefault();
        }
        public T QueryFirst(string baseQuery, DynamicParameters parameters, string orderBy = "")
        {
            return Query(baseQuery, parameters, orderBy, ONE_RESULT).FirstOrDefault();
        }

        public T QueryFirst<T1>(string baseQuery, Func<T, T1, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id", string orderBy = "")
        {
            return Query(baseQuery, transformationFunction, parameters, splitOn, orderBy, ONE_RESULT).FirstOrDefault();
        }

        public T QueryFirst<T1, T2>(string baseQuery, Func<T, T1, T2, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id", string orderBy = "")
        {
            return Query(baseQuery, transformationFunction, parameters, splitOn, orderBy, ONE_RESULT).FirstOrDefault();
        }

        public T QueryFirst<T1, T2, T3>(string baseQuery, Func<T, T1, T2, T3, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id, Id", string orderBy = "")
        {
            return Query(baseQuery, transformationFunction, parameters, splitOn, orderBy, ONE_RESULT).FirstOrDefault();
        }

        public T QueryFirst<T1, T2, T3, T4>(string baseQuery, Func<T, T1, T2, T3, T4, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id, Id, Id", string orderBy = "")
        {
            return Query(baseQuery, transformationFunction, parameters, splitOn, orderBy, ONE_RESULT).FirstOrDefault();
        }
        
        public T QueryFirst<T1, T2, T3, T4, T5>(string baseQuery, Func<T, T1, T2, T3, T4, T5, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id, Id, Id, Id", string orderBy = "")
        {
            return Query(baseQuery, transformationFunction, parameters, splitOn, orderBy, ONE_RESULT).FirstOrDefault();
        }

        public bool Delete(object id)
        {
            var entity = _dbContext.Connection.Get<T>(id);
            return _dbContext.Connection.Delete(entity, _dbContext.Transaction);
        }
    }
}