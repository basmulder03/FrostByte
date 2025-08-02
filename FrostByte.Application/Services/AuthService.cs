using FrostByte.Application.Configuration;
using Microsoft.Extensions.Logging;

namespace FrostByte.Application.Services;

public class AuthService : IAuthService
{
    private const string CookieKey = "AocSessionCookie";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<AuthService> _logger;
    private readonly ISecretStore _secretStore;

    public AuthService(ISecretStore secretStore, IHttpClientFactory httpClientFactory, ILogger<AuthService> logger)
    {
        _secretStore = secretStore ?? throw new ArgumentNullException(nameof(secretStore));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _logger.LogInformation("AuthService initialized with secret store and HTTP client factory.");
    }

    public Task<string?> GetSessionCookieAsync()
    {
        _logger.LogDebug("Retrieving session cookie from secret store with key: {CookieKey}", CookieKey);
        return _secretStore.GetAsync(CookieKey);
    }

    public Task StoreSessionCookieAsync(string cookie, DateTimeOffset? expiresUtc = null)
    {
        _logger.LogDebug("Storing session cookie with key: {CookieKey}, expires at: {ExpiresUtc}", CookieKey,
            expiresUtc);
        return _secretStore.SetAsync(CookieKey, cookie, expiresUtc);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var cookie = await GetSessionCookieAsync();
        if (!string.IsNullOrEmpty(cookie)) return true;
        _logger.LogWarning("Session cookie is null or empty. User is not authenticated.");
        return false;
    }

    public Task InvalidateAsync()
    {
        _logger.LogInformation("Invalidating session by removing cookie with key: {CookieKey}", CookieKey);
        return _secretStore.RemoveAsync(CookieKey);
    }
}
