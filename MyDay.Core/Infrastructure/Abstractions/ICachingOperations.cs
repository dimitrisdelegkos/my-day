namespace MyDay.Core.Infrastructure.Abstractions
{
    public interface ICachingOperations
    {
        Task<(bool, T)> GetSetCacheEntry<T>(string cacheKey, Func<Task<T>> seeder, int cacheDuration = 0); 
    }
}