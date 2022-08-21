using Dapper;

namespace Nexttag.Database
{
    public interface IRepository<T>
    {

        public abstract string Insert(T entity);
        public abstract T Update(T entity);
        public abstract bool Delete(string id);
        public abstract IEnumerable<T> Query(string baseQuery, IEnumerable<IFilter> internalFilters, string orderBy = "", PaginationContext pagination = null);
        public abstract IEnumerable<T> Query<T1>(string baseQuery, Func<T, T1, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id", string orderBy = "", PaginationContext pagination = null);
        public abstract IEnumerable<T> Query<T1, T2>(string baseQuery, Func<T, T1, T2, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id, Id", string orderBy = "", PaginationContext pagination = null);
        public abstract IEnumerable<T> Query<T1, T2, T3>(string baseQuery, Func<T, T1, T2, T3, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id, Id, Id", string orderBy = "", PaginationContext pagination = null);
        public abstract IEnumerable<T> Query<T1, T2, T3, T4>(string baseQuery, Func<T, T1, T2, T3, T4, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id, Id, Id, Id", string orderBy = "", PaginationContext pagination = null);

        public abstract IEnumerable<T> Query<T1, T2, T3, T4, T5>(string baseQuery, Func<T, T1, T2, T3, T4, T5, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id, Id, Id, Id, Id", string orderBy = "", PaginationContext pagination = null);

        public bool Delete(object id);

        public abstract T QueryFirst(string baseQuery, IEnumerable<IFilter> filters, string orderBy = "");
        public abstract T QueryFirst<T1>(string baseQuery, Func<T, T1, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id", string orderBy = "");
        public abstract T QueryFirst<T1, T2>(string baseQuery, Func<T, T1, T2, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id, Id", string orderBy = "");
        public abstract T QueryFirst<T1, T2, T3>(string baseQuery, Func<T, T1, T2, T3, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id, Id, Id", string orderBy = "");
        public abstract T QueryFirst<T1, T2, T3, T4>(string baseQuery, Func<T, T1, T2, T3, T4, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id, Id, Id, Id", string orderBy = "");
        public abstract T QueryFirst<T1, T2, T3, T4, T5>(string baseQuery, Func<T, T1, T2, T3, T4, T5, T> transformationFunction, IEnumerable<IFilter> internalFilters, string splitOn = "Id, Id, Id, Id, Id, Id", string orderBy = "");


        public abstract IEnumerable<T> Query(string baseQuery, DynamicParameters parameters, string orderBy = "", PaginationContext pagination = null);
        public abstract IEnumerable<T> Query<T1>(string baseQuery, Func<T, T1, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id", string orderBy = "", PaginationContext pagination = null);
        public abstract IEnumerable<T> Query<T1, T2>(string baseQuery, Func<T, T1, T2, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id", string orderBy = "", PaginationContext pagination = null);
        public abstract IEnumerable<T> Query<T1, T2, T3>(string baseQuery, Func<T, T1, T2, T3, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id, Id", string orderBy = "", PaginationContext pagination = null);
        public abstract IEnumerable<T> Query<T1, T2, T3, T4>(string baseQuery, Func<T, T1, T2, T3, T4, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id, Id, Id", string orderBy = "", PaginationContext pagination = null);
        public abstract IEnumerable<T> Query<T1, T2, T3, T4, T5>(string baseQuery, Func<T, T1, T2, T3, T4, T5, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id, Id, Id, Id", string orderBy = "", PaginationContext pagination = null);

        public abstract T QueryFirst(string baseQuery, DynamicParameters parameters, string orderBy = "");
        public abstract T QueryFirst<T1>(string baseQuery, Func<T, T1, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id", string orderBy = "");
        public abstract T QueryFirst<T1, T2>(string baseQuery, Func<T, T1, T2, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id", string orderBy = "");
        public abstract T QueryFirst<T1, T2, T3>(string baseQuery, Func<T, T1, T2, T3, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id, Id", string orderBy = "");
        public abstract T QueryFirst<T1, T2, T3, T4>(string baseQuery, Func<T, T1, T2, T3, T4, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id, Id, Id", string orderBy = "");
        public abstract T QueryFirst<T1, T2, T3, T4, T5>(string baseQuery, Func<T, T1, T2, T3, T4, T5, T> transformationFunction, DynamicParameters parameters, string splitOn = "Id, Id, Id, Id, Id, Id", string orderBy = "");

    }
}