using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Text;

namespace Microsoft.Extensions.Caching
{
    public class CacheManager : ICacheManager
    {
        private readonly IDistributedCache _cache;

        public CacheManager(IDistributedCache cache) => _cache = cache;

        /// <summary>
        /// But in large-scale applications where we face a lot of calls that must be cached, it is better to create a static field
        /// </summary>
        private static JsonSerializerSettings _serializerOptions = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
        };

        /// <summary>
        /// But in large-scale applications where we face a lot of calls that must be cached, it is better to create a static field
        /// </summary>
        private static DistributedCacheEntryOptions _distributedCacheEntryOptions = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(10), //expira se ficar mais de 10 minutos sem usar o cache
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12) //expira depois de 12h do cache criado.
        };

        /// <summary>
        /// Compress byte for use em cache with GZipStream
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        private static async Task<byte[]> CompressBytesAsync(byte[] bytes, CancellationToken cancel = default(CancellationToken))
        {
            try
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var compressionStream = new GZipStream(outputStream, CompressionLevel.Optimal))
                    {
                        await compressionStream.WriteAsync(bytes, 0, bytes.Length, cancel);
                    }
                    return outputStream.ToArray();
                }
            }
            catch
            {
                return bytes;
            }
        }

        /// <summary>
        /// Decompress byte from cache with GZipStream
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        private static async Task<byte[]> DecompressBytesAsync(byte[] bytes, CancellationToken cancel = default(CancellationToken))
        {
            try
            {
                using (var inputStream = new MemoryStream(bytes))
                {
                    using (var outputStream = new MemoryStream())
                    {
                        using (var compressionStream = new GZipStream(inputStream, CompressionMode.Decompress))
                        {
                            await compressionStream.CopyToAsync(outputStream, cancel);
                        }
                        return outputStream.ToArray();
                    }
                }
            }
            catch
            {
                return bytes;
            }
        }

        public async Task<T?> GetAsync<T>(string cacheName) where T : class
        {
            var encodedCached = await _cache.GetAsync(cacheName);

            if (encodedCached != null)
            {
                var cached = Encoding.UTF8.GetString(await DecompressBytesAsync(encodedCached));
                if (!string.IsNullOrEmpty(cached))
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(cached, _serializerOptions);
            }

            return null;
        }

        public async Task<T> SetAsync<T>(string cacheName, T value, DistributedCacheEntryOptions? options = null) where T : class
        {
            if (options == null)
                options = _distributedCacheEntryOptions;

            byte[] encodedCurrent = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(value, _serializerOptions));
            await _cache.SetAsync(cacheName, await CompressBytesAsync(encodedCurrent), options);

            return value;
        }

        public async Task<T> GetOrCreateAsync<T>(string cacheName, Func<Task<T>> factory) where T : class
        {
            var cached = await GetAsync<T>(cacheName);
            if (cached != null)
                return cached;

            var generatedValue = await factory();
            return await SetAsync<T>(cacheName, generatedValue, _distributedCacheEntryOptions);
        }

        public async Task<T> GetOrCreateAsync<T>(string cacheName, DistributedCacheEntryOptions options, Func<Task<T>> factory) where T : class
        {
            var cached = await GetAsync<T>(cacheName);
            if (cached != null)
                return cached;

            var generatedValue = await factory();
            return await SetAsync<T>(cacheName, generatedValue, options);
        }

        public bool TryGetValue<T>(string cacheName, out T? value) where T : class
        {
            value = GetAsync<T>(cacheName).GetAwaiter().GetResult();
            if (value == null) return false;
            return true;
        }
    }
}