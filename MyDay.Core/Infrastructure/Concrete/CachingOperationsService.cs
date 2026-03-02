using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyDay.Core.Infrastructure.Abstractions;
using System.Text.Json;

namespace MyDay.Core.Infrastructure.Concrete
{
    public class CachingOperationsService : ICachingOperations
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<CachingOperationsService> _logger;

        public CachingOperationsService(IConfiguration configuration,
            IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            ILogger<CachingOperationsService> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _memoryCache = memoryCache ?? throw new ArgumentException(nameof(memoryCache));
            _distributedCache = distributedCache ?? throw new ArgumentException(nameof(distributedCache));
            _logger = logger ?? throw new ArgumentException(nameof(distributedCache));
        }

        public async Task<(bool, T)> GetSetCacheEntry<T>(string cacheKey, Func<Task<T>> seeder, int cacheDuration = 0)
        {
            try
            {
                string cachingMode = _configuration.GetValue<string>("CacheSettings:Mode");
                if (cacheDuration == 0) cacheDuration = _configuration.GetValue<int>("CacheSettings:Timeout");

                if(cachingMode == "InMemory")
                {
                    var cacheValue = await this.GetSetFromMemoryCache(cacheKey, seeder, TimeSpan.FromMinutes(cacheDuration));
                    return (true, cacheValue);
                }
                else if (cachingMode == "Distributed")
                {
                    var cacheValue = await this.GetSetFromDistributedCache(cacheKey, seeder, TimeSpan.FromMinutes(cacheDuration));
                    return (true, cacheValue);
                }
                else
                {
                    throw new Exception("Unsupported caching mode was set.");
                }  
            }
            catch (Exception exception) 
            {
                _logger.LogError("Could proceed with caching operation for {cacheKey}, Exception: {Error}", cacheKey, exception.Message);
                return (false, default(T));
            }
        }

        #region Helpers

        private async Task<T> GetSetFromMemoryCache<T>(string cacheKey, Func<Task<T>> seeder, TimeSpan cacheDuration)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out T cacheValue))
            {
                cacheValue = await seeder();
                _memoryCache.Set(cacheKey, cacheValue, cacheDuration);
            }

            return cacheValue;
        }

        private async Task<T> GetSetFromDistributedCache<T>(string cacheKey, Func<Task<T>> seeder, TimeSpan cacheDuration)
        {
            var cacheValue = await _distributedCache.GetStringAsync(cacheKey);
            if (cacheValue != null)
            {
                return JsonSerializer.Deserialize<T>(cacheValue);
            }

            var newCacheValue = await seeder();
            await _distributedCache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(newCacheValue),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = cacheDuration
                });

            return newCacheValue; 
        }

        #endregion
    }
}
