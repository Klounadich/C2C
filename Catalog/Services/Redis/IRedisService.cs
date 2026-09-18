namespace Catalog.Services.Redis;

public interface IRedisService
{
    public Task IncrementAsync(Guid itemId);
    public Task<long> GetAsync(Guid itemId);
}