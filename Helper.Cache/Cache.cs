using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Helper.Cache
{
    /// <summary>
    /// Cache represents a cache stored in the memory of the web server.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class Cache<TItem> : ICache<TItem>
    {
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private readonly ConcurrentDictionary<object, SemaphoreSlim> _locks
            = new ConcurrentDictionary<object, SemaphoreSlim>();

        public async Task<TItem> GetOrCreate(object key, Func<Task<TItem>> createItem)
        {
            TItem cacheEntry;

            if (!_cache.TryGetValue(key, out cacheEntry))
            {
                SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

                await mylock.WaitAsync();

                try
                {
                    if (!_cache.TryGetValue(key, out cacheEntry))
                    {
                        cacheEntry = await createItem();

                        _cache.Set(key, cacheEntry);
                    }
                }
                finally
                {
                    mylock.Release();
                }
            }
            return cacheEntry;
        }
    }
}
