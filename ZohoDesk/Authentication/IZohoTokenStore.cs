using ZohoDesk.Models;

namespace ZohoDesk.Authentication;

public interface IZohoTokenStore
{
    Task<ZohoToken?> GetAsync(CancellationToken cancellationToken = default);

    Task SaveAsync(
        ZohoToken token,
        CancellationToken cancellationToken = default);

    Task RemoveAsync(CancellationToken cancellationToken = default);
}