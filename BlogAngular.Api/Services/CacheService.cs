using Microsoft.Extensions.Caching.Memory;

namespace BlogAngular.Api.Services
{
    public class CacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void SetCache(string key, object value, TimeSpan expiration)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration // Thời gian hết hạn
            };

            _memoryCache.Set(key, value, cacheEntryOptions);
        }

        public T GetCache<T>(string key)
        {
            return _memoryCache.TryGetValue(key, out T value) ? value : default;
        }

        public void RemoveCache(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
