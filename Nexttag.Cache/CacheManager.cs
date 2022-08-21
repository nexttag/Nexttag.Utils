using System.Runtime.Caching;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nexttag.Cache.Options;

namespace Nexttag.Cache
{
    public class CacheManager : ICacheManager
    {
        private TimeSpan DefaulExpirationTime = TimeSpan.FromMinutes(1);
        private readonly ILogger<CacheManager> _logger;
        private ObjectCache _objectCache;
        private readonly MemoryCache _memoryCache;
        private readonly CacheManagerOptions _options;
        private Timer _timer;

        internal ObjectCache CurrentObjectCache { get; private set; }


        public CacheManager(ObjectCache objectCache, IOptions<CacheManagerOptions> options, ILogger<CacheManager> logger)
        {
            _logger = logger;
            _objectCache = objectCache;
            _memoryCache = new MemoryCache("MemoryCache");

            _options = options.Value ?? new CacheManagerOptions()
            {
                EntityNameAlias = new Dictionary<string, string>(),
                TTL = new Dictionary<string, int>()
            };
            CurrentObjectCache = objectCache ?? _memoryCache;
        }

        public void Add<TEntity>(long id, TEntity entity, string region = null, int? expiration = null) where TEntity : class
        {
            Add(id.ToString(), entity, region, expiration);
        }

        public void Update<TEntity>(long id, TEntity entity, string region = null, int? expiration = null) where TEntity : class
        {
            Add(id.ToString(), entity, region, expiration);
        }

        public void Update<TEntity>(string id, TEntity entity, string region = null, int? expiration = null) where TEntity : class
        {
            Add(id, entity, region, expiration);
        }

        public void Remove<TEntity>(long id, string region = null) where TEntity : class
        {
            Remove<TEntity>(id.ToString(), region);
        }

        public TEntity GetById<TEntity>(long id, string region = null) where TEntity : class
        {
            return GetById<TEntity>(id.ToString(), region);
        }

        public void Add<TEntity>(string id, TEntity entity, string region = null, int? expiration = null) where TEntity : class
        {
            try
            {
                if (CurrentObjectCache.DefaultCacheCapabilities.HasFlag(DefaultCacheCapabilities.CacheRegions))
                {
                    CurrentObjectCache.Set(new CacheItem(KeyFromId<TEntity>(id), entity, region), GetCachePolicy<TEntity>(expiration));
                }
                else
                {
                    CurrentObjectCache.Set(new CacheItem(KeyFromId<TEntity>(id), entity), GetCachePolicy<TEntity>(expiration));
                }
            }
            catch (Exception ex)
            {
                HandlerError(ex);
                Add(id, entity, region, expiration);
            }
        }

        public void Remove<TEntity>(string id, string region = null) where TEntity : class
        {
            try
            {
                if (CurrentObjectCache.DefaultCacheCapabilities.HasFlag(DefaultCacheCapabilities.CacheRegions))
                {
                    CurrentObjectCache.Remove(KeyFromId<TEntity>(id), region);
                }
                else
                {
                    CurrentObjectCache.Remove(KeyFromId<TEntity>(id));
                }
            }
            catch (Exception ex)
            {
                HandlerError(ex);
            }
        }

        public TEntity GetById<TEntity>(string id, string region = null) where TEntity : class
        {
            TEntity entity;
            try
            {
                if (CurrentObjectCache.DefaultCacheCapabilities.HasFlag(DefaultCacheCapabilities.CacheRegions))
                {
                    entity = (TEntity)CurrentObjectCache.Get(KeyFromId<TEntity>(id), region);
                }
                else
                {
                    entity = (TEntity)CurrentObjectCache.Get(KeyFromId<TEntity>(id));
                }
            }
            catch (Exception ex)
            {
                HandlerError(ex);
                entity = GetById<TEntity>(id, region);
            }
            return entity;
        }

        public IEnumerable<TEntity> GetAll<TEntity>(string region = null) where TEntity : class
        {
            IEnumerable<TEntity> entities = new List<TEntity>(0);
            try
            {
                if (CurrentObjectCache.DefaultCacheCapabilities.HasFlag(DefaultCacheCapabilities.InMemoryProvider))
                {
                    entities = CurrentObjectCache.Where(k => k.Value is TEntity).Select(k => (TEntity)k.Value);
                }
                else
                {
                    var entityName = typeof(TEntity).Name;
                    entities = (CurrentObjectCache.Get($"{entityName}:*") as List<object>).Select(k => (TEntity)k);
                }
            }
            catch (Exception ex)
            {
                HandlerError(ex);
                entities = GetAll<TEntity>(region);
            }
            return entities;
        }

        private CacheItemPolicy GetCachePolicy<TEntity>(int? expiration)
        {
            CacheItemPolicy policy = null;

            if (CurrentObjectCache.DefaultCacheCapabilities.HasFlag(DefaultCacheCapabilities.AbsoluteExpirations))
            {
                var _entityName = typeof(TEntity).Name;
                var entityExpirationTime = DefaulExpirationTime;
                if (_options.TTL.ContainsKey(_entityName) || expiration.HasValue)
                    entityExpirationTime = TimeSpan.FromMinutes(expiration ?? _options.TTL[_entityName]);
                policy = new CacheItemPolicy()
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now).Add(entityExpirationTime)
                };
            }

            return policy;
        }

        private string KeyFromId<TEntity>(string id)
        {
            var entityName = typeof(TEntity).Name;
            if (_options.EntityNameAlias.ContainsKey(entityName))
                entityName = _options.EntityNameAlias[entityName];
            return $"{entityName}:{id}";
        }
        private void HandlerError(Exception ex)
        {
            _logger.LogError($"Error on {CurrentObjectCache}: {ex.Message}", ex);
            CurrentObjectCache = _memoryCache;
            _timer = new Timer(
                 state =>
                 {
                     CurrentObjectCache = _objectCache;
                     _timer.Dispose();
                     _timer = null;
                 },
                 null,
                 DefaulExpirationTime,
                 Timeout.InfiniteTimeSpan
            );
        }

    }
}