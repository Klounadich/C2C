using StackExchange.Redis;

namespace Catalog.Services.Redis;

public class RedisService : IRedisService
{
    private readonly IDatabase _redis;

    public RedisService(IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
    }
    public async Task IncrementAsync(Guid itemId)
    {
        await _redis.StringIncrementAsync($"item:{itemId}:views");
    }

    public async Task<long> GetAsync(Guid itemId)
    {
        var value = await _redis.StringGetAsync($"item:{itemId}:views");

        return value.HasValue ? (long)value : 0;
    }
}