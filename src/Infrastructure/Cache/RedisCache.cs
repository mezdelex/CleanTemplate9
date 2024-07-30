using System.Text.Json;
using Domain.Cache;
using StackExchange.Redis;

namespace Infrastructure.Cache;

public sealed class RedisCache : IRedisCache
{
    private readonly IDatabase _redisDB;

    public RedisCache(IDatabase redisDB)
    {
        _redisDB = redisDB;
    }

    public async Task<T?> GetCachedData<T>(string key)
    {
        var value = await _redisDB.StringGetAsync(key);
        if (value.HasValue)
            return JsonSerializer.Deserialize<T>(value.ToString());

        return default;
    }

    public async Task SetCachedData<T>(string key, T value, DateTimeOffset dateTimeOffset)
    {
        var expirationTime = dateTimeOffset.DateTime.Subtract(DateTime.Now);
        await _redisDB.StringSetAsync(key, JsonSerializer.Serialize<T>(value), expirationTime);
    }

    public async Task RemoveData<T>(string key)
    {
        var keyExists = await _redisDB.KeyExistsAsync(key);
        if (keyExists)
            await _redisDB.KeyDeleteAsync(key);
    }
}
