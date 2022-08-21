using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nexttag.Cache.Options;

namespace Nexttag.Cache.Extensions
{
    public static class CacheManagerExtensions
    {
        public static IServiceCollection AddCacheManager(this IServiceCollection services, Action<CacheManagerOptions> setupAction = null)
        {
            services.AddOptions();
            services.AddSingleton<IConfigureOptions<CacheManagerOptions>, ConfigureCacheManagerOptions>();

            services.AddSingleton(typeof(ICacheManager), typeof(CacheManager));

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return services;
        }
    }
}