using System.Collections.Generic;

namespace Nexttag.Utils.Cache.Options
{
    public class CacheManagerOptions
    {
        public const string CacheManager = "Cache";
        public Dictionary<string, int> TTL { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, string> EntityNameAlias { get; set; } = new Dictionary<string, string>();
    }
}