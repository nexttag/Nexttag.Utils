using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Nexttag.Utils.Database
{
    public class DatabaseContext
    {

        private readonly IDbParameterBuilder _dbParameterBuilder;

        public DatabaseContext(IDbConnection connection, IDbParameterBuilder dbParameterBuilder)
        {
            Connection = connection;
            _dbParameterBuilder = dbParameterBuilder;
        }

        internal IDbConnection Connection { get; private set; }

        internal IDbTransaction Transaction { get; private set; }

        internal DynamicParameters BuildParameters(IEnumerable<IFilter> filters)
        {
            return _dbParameterBuilder.BuildParameters(filters);
        }

        internal string BuildClauses(IEnumerable<IFilter> filters)
        {
            return _dbParameterBuilder.BuildClauses(filters);
        }

        internal bool BeginTransaction()
        {
            if (Transaction == null)
            {
                Transaction = Connection.BeginTransaction();
                return true;
            }

            return false;
        }

        internal void CommitTransaction()
        {
            Transaction.Commit();
            Transaction = null;
        }

        internal void RollbackTransaction()
        {
            Transaction.Rollback();
            Transaction = null;
        }

        internal bool OpenConnection()
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
                return true;
            }
            return false;
        }

        internal void CloseConnection()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
        }

        internal string BuildQuery(string baseQuery, string orderBy, PaginationContext pagination)
        {
            return _dbParameterBuilder.BuildQuery(baseQuery, orderBy, pagination);
        }
    }
}