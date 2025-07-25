namespace FrostByte.Application.Configuration;

public interface ISecretStore
{
    Task SetAsync(string key, string value, DateTimeOffset? expiresUtc = null);
    Task<string?> GetAsync(string key);
    Task RemoveAsync(string key);
}
