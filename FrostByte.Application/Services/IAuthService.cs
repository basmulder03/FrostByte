using Microsoft.Maui.Controls;

namespace FrostByte.Application.Services;

public interface IAuthService
{
    /// <summary>
    /// Returns current AoC session token or null.
    /// </summary>
    Task<string?> GetSessionCookieAsync();

    /// <summary>
    /// Stores the session cookie in a secure store.
    /// </summary>
    Task StoreSessionCookieAsync(string cookie, DateTimeOffset? expiresUtc = null);

    /// <summary>
    /// Returns true if we ahve a valid, non-expired session token.
    /// </summary>
    Task<bool> IsAuthenticatedAsync();

    /// <summary>
    /// Clears the current session token and any related data.
    /// </summary>
    Task InvalidateAsync();
}
