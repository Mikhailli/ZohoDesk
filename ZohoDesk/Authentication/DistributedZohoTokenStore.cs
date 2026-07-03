using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using ZohoDesk.Models;

namespace ZohoDesk.Authentication;

public sealed class DistributedZohoTokenStore(IDistributedCache cache) : IZohoTokenStore
{
    private const string CacheKey = "ZohoDesk:AccessToken";

    private readonly IDistributedCache _cache = cache;

    public async Task<ZohoToken?> GetAsync(CancellationToken cancellationToken = default)
    {
        var json = await _cache.GetStringAsync(CacheKey, cancellationToken);

        if (string.IsNullOrWhiteSpace(json))
            return null;

        return JsonSerializer.Deserialize<ZohoToken>(json);
    }

    public async Task SaveAsync(
        ZohoToken token,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(token);

        await _cache.SetStringAsync(
            CacheKey,
            json,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = token.ExpiresAtUtc
            },
            cancellationToken);
    }

    public Task RemoveAsync(CancellationToken cancellationToken = default)
    {
        return _cache.RemoveAsync(CacheKey, cancellationToken);
    }
}