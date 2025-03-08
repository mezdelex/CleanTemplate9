namespace Domain.Cache;

public interface IRedisCache
{
    Task<T?> GetCachedData<T>(string key);
    Task SetCachedData<T>(string key, T value, DateTimeOffset expirationTime);
    Task RemoveKeysByPattern(string key);
}
