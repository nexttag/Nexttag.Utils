namespace Nexttag.Cache
{
    public interface ICacheManager
    {
        void Add<TEntity>(long id, TEntity data, string region = null, int? expiration = null) where TEntity : class;
        void Add<TEntity>(string id, TEntity data, string region = null, int? expiration = null) where TEntity : class;
        void Update<TEntity>(long id, TEntity entity, string region = null, int? expiration = null) where TEntity : class;
        void Update<TEntity>(string id, TEntity data, string region = null, int? expiration = null) where TEntity : class;
        void Remove<TEntity>(long id, string region = null) where TEntity : class;
        void Remove<TEntity>(string id, string region = null) where TEntity : class;
        TEntity GetById<TEntity>(long id, string region = null) where TEntity : class;
        TEntity GetById<TEntity>(string id, string region = null) where TEntity : class;
        IEnumerable<TEntity> GetAll<TEntity>(string region = null) where TEntity : class;
    }
}