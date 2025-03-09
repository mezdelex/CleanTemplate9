namespace Infrastructure.Cache;

public sealed class RedisCache(
    IDatabase redisDatabase,
    IConnectionMultiplexer connectionMultiplexer
) : IRedisCache
{
    public async Task<T?> GetCachedData<T>(string key)
    {
        var value = await redisDatabase.StringGetAsync(key);

        return value.HasValue ? JsonSerializer.Deserialize<T>(value.ToString()) : default;
    }

    public async Task SetCachedData<T>(string key, T value, DateTimeOffset dateTimeOffset)
    {
        var expirationTime = dateTimeOffset.DateTime.Subtract(DateTime.Now);

        await redisDatabase.StringSetAsync(
            key,
            JsonSerializer.Serialize(value, value!.GetType()),
            expirationTime
        );
    }

    public async Task RemoveKeysByPattern(string pattern)
    {
        var keys = new List<RedisKey>();
        foreach (var server in connectionMultiplexer.GetServers())
        {
            await foreach (var key in server.KeysAsync(pattern: $"*{pattern}*"))
            {
                keys.Add(key);
            }
        }

        if (keys.Count > 0)
        {
            await redisDatabase.KeyDeleteAsync([.. keys]);
        }
    }
}
