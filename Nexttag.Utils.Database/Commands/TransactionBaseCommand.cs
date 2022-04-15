using System;

namespace Nexttag.Utils.Database.Commands
{
    public abstract class TransactionBaseCommand<In, Out> : BaseCommand<In, Out>
    {
        private bool _openTransaction = false;

        protected TransactionBaseCommand(DatabaseContextContainer repository) : base(repository)
        {

        }

        public new Out Execute(In entity)
        {
            DatabaseContext.OpenConnection();

            _openTransaction = DatabaseContext.BeginTransaction();

            Out result = default(Out);

            try
            {
                result = InternalExecute(entity);

                if (_openTransaction)
                {
                    DatabaseContext.CommitTransaction();
                }
            }
            catch (Exception)
            {
                if (_openTransaction)
                {
                    DatabaseContext.RollbackTransaction();
                }
                throw;
            }
            finally
            {
                if (_openTransaction)
                {
                    DatabaseContext.CloseConnection();
                }
            }

            return result;
        }

    }
}