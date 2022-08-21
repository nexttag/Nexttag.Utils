using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Nexttag.Cache.Options
{
    public class ConfigureCacheManagerOptions : ConfigureFromConfigurationOptions<CacheManagerOptions>
    {
        private readonly IConfiguration _configuration;

        public ConfigureCacheManagerOptions(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public override void Configure(CacheManagerOptions options)
        {
            base.Configure(options);

            _configuration.GetSection("Cache").Bind(options);
        }
    }
}