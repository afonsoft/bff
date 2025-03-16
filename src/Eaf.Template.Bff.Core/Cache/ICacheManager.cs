using Microsoft.Extensions.Caching.Distributed;

namespace Microsoft.Extensions.Caching
{
    public interface ICacheManager
    {
        /// <summary>
        /// Get a Cache from a Key
        /// </summary>
        /// <typeparam name="T">typeof Class</typeparam>
        /// <param name="cacheName">Key name of Cache</param>
        /// <returns></returns>
        Task<T?> GetAsync<T>(string cacheName) where T : class;

        /// <summary>
        /// try Get a Cache from a Key
        /// </summary>
        /// <typeparam name="T">ypeof Class</typeparam>
        /// <param name="cacheName">Key name of Cache</param>
        /// <param name="value">output class of cache</param>
        /// <returns>boolean result</returns>
        bool TryGetValue<T>(string cacheName, out T? value) where T : class;

        /// <summary>
        /// Working with Cache (IDistributedCache), Set a Cache from a Kay and Object
        /// </summary>
        /// <typeparam name="T">typeof Class</typeparam>
        /// <param name="cacheName">Key name of Cache</param>
        /// <param name="value">the object for cache </param>
        /// <param name="options">options <see cref="DistributedCacheEntryOptions"> of <see cref="IDistributedCache"/> </param>
        /// <returns></returns>
        Task<T> SetAsync<T>(string cacheName, T value, DistributedCacheEntryOptions? options = null) where T : class;

        /// <summary>
        /// Working with Cache (IDistributedCache), if is null or expiate create new cache from action
        /// </summary>
        /// <typeparam name="T">typeof Class</typeparam>
        /// <param name="cacheName">Key name of Cache</param>
        /// <param name="factory">Function for Cache</param>
        /// <returns></returns>
        Task<T> GetOrCreateAsync<T>(string cacheName, Func<Task<T>> factory) where T : class;

        /// <summary>
        /// Working with Cache (IDistributedCache), if is null or expiate create new cache from action
        /// </summary>
        /// <typeparam name="T">typeof Class</typeparam>
        /// <param name="cacheName">Key name of Cache</param>
        /// <param name="options">options <see cref="DistributedCacheEntryOptions"> of <see cref="IDistributedCache"/> </param>
        /// <param name="factory">Function for Cache</param>
        /// <returns></returns>
        Task<T> GetOrCreateAsync<T>(string cacheName, DistributedCacheEntryOptions options, Func<Task<T>> factory) where T : class;
    }
}