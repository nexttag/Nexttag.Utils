namespace Nexttag.Database
{
    public abstract class DatabaseContextContainer
    {
        private readonly DatabaseContext _dbContext;

        protected DatabaseContextContainer(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DatabaseContext GetCurrentDatabaseContext()
        {
            return _dbContext;
        }
    }
}