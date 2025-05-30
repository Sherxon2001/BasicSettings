namespace BasicSettings.DataLayer.Repository.Repositories.Cache.Concrete
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IMemoryCache _memoryCache;

        public CacheRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Value GetValueFromCache<TKey, Value>(TKey key)
        {
            if (_memoryCache.TryGetValue(key, out Value value))
            {
                return value;
            }
            return default;
        }

        public void SetValueToCache<TKey, Value>(TKey key, Value value, TimeSpan? timeSpan = null)
        {
            if (timeSpan == null)
            {
                timeSpan = TimeSpan.FromHours(1);
            }
            _memoryCache.Set(key, value, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = timeSpan });
        }
    }
}
