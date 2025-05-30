namespace BasicSettings.DataLayer.Repository.Repositories.Contract
{
    public interface ICacheRepository
    {
        void SetValueToCache<TKey, Value>(TKey key, Value value, TimeSpan? timeSpan = null);
        Value GetValueFromCache<TKey, Value>(TKey key);
    }
}
