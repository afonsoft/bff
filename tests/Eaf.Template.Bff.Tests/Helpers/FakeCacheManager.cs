using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Concurrent;

namespace Eaf.Template.Bff.Tests.Helpers;

/// <summary>
/// Implementação fake de ICacheManager para testes, mantendo cache em memória.
/// </summary>
public class FakeCacheManager : ICacheManager
{
    private readonly ConcurrentDictionary<string, object> _store = new();

    public Task<T?> GetAsync<T>(string cacheName) where T : class
    {
        if (_store.TryGetValue(cacheName, out var value) && value is T typed)
            return Task.FromResult<T?>(typed);

        return Task.FromResult<T?>(null);
    }

    public Task<T> SetAsync<T>(string cacheName, T value, DistributedCacheEntryOptions? options = null) where T : class
    {
        _store[cacheName] = value;
        return Task.FromResult(value);
    }

    public async Task<T> GetOrCreateAsync<T>(string cacheName, Func<Task<T>> factory) where T : class
    {
        var cached = await GetAsync<T>(cacheName);
        if (cached != null)
            return cached;

        var value = await factory();
        return await SetAsync(cacheName, value);
    }

    public async Task<T> GetOrCreateAsync<T>(string cacheName, DistributedCacheEntryOptions options, Func<Task<T>> factory) where T : class
    {
        return await GetOrCreateAsync(cacheName, factory);
    }

    public bool TryGetValue<T>(string cacheName, out T? value) where T : class
    {
        value = GetAsync<T>(cacheName).GetAwaiter().GetResult();
        return value != null;
    }
}
