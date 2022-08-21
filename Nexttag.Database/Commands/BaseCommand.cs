namespace Nexttag.Database.Commands
{
    public abstract class BaseCommand<In, Out>
    {

        protected DatabaseContext DatabaseContext { get; private set; }

        protected BaseCommand(DatabaseContextContainer databaseContext)
        {
            DatabaseContext = databaseContext.GetCurrentDatabaseContext();
        }

        public Out Execute(In entity)
        {
            var needCloseConnection = DatabaseContext.OpenConnection();

            var result = InternalExecute(entity);

            if (needCloseConnection)
            {
                DatabaseContext.CloseConnection();
            }

            return result;
        }

        public abstract Out InternalExecute(In entity);

    }
}