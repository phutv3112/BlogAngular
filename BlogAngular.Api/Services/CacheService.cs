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
        private class CacheEntry
        {
            public object Value { get; set; } // Dữ liệu được lưu trong cache
            public DateTime ExpirationTime { get; set; } // Thời gian hết hạn
        }
        public void SetCacheWithExpiration(string key, object value, TimeSpan expiration)
        {
            var expirationTime = DateTime.UtcNow.Add(expiration);

            var cacheEntry = new CacheEntry
            {
                Value = value,
                ExpirationTime = expirationTime
            };

            _memoryCache.Set(key, cacheEntry, expiration);
        }


        public T GetCache<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out CacheEntry cacheEntry))
            {
                return (T)cacheEntry.Value;
            }

            return default;
        }
        public bool IsCacheKeyValid(string key)
        {
            if (_memoryCache.TryGetValue(key, out CacheEntry cacheEntry))
            {
                return cacheEntry.ExpirationTime > DateTime.UtcNow;
            }

            return false; // Key không tồn tại hoặc đã hết hạn
        }

        public void RemoveCache(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
