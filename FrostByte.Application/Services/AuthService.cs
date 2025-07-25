using System.Net;
using FrostByte.Application.Configuration;

namespace FrostByte.Application.Services;

public class AuthService : IAuthService
{
    private const string CookieKey = "AocSessionCookie";
    private readonly ISecretStore _secretStore;
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthService(ISecretStore secretStore, IHttpClientFactory httpClientFactory)
    {
        _secretStore = secretStore ?? throw new ArgumentNullException(nameof(secretStore));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public Task<string?> GetSessionCookieAsync()
    {
        return _secretStore.GetAsync(CookieKey);
    }

    public Task StoreSessionCookieAsync(string cookie, DateTimeOffset? expiresUtc = null)
    {
        return _secretStore.SetAsync(CookieKey, cookie, expiresUtc);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var cookie = await GetSessionCookieAsync();
        if (string.IsNullOrEmpty(cookie))
        {
            return false;
        }

        var client = _httpClientFactory.CreateClient("AoC");
        const string address = "https://adventofcode.com/2015/stats";
        var resp = await client.GetAsync(address);
        return resp.StatusCode == HttpStatusCode.OK;
    }

    public Task InvalidateAsync()
    {
        return _secretStore.RemoveAsync(CookieKey);
    }

    internal Task StoreSessionAsync(string cookie, DateTimeOffset? expires = null)
    {
        return _secretStore.SetAsync(CookieKey, cookie, expires);
    }
}
