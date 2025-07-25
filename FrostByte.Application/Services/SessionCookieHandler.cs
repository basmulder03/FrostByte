using Microsoft.Extensions.Logging;

namespace FrostByte.Application.Services;

public class SessionCookieHandler(IAuthService authService, ILogger<SessionCookieHandler> logger) : DelegatingHandler
{
    private readonly IAuthService _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    private readonly ILogger<SessionCookieHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var cookie = await _authService.GetSessionCookieAsync();
        request.Headers.Remove("Cookie");
        if (string.IsNullOrEmpty(cookie)) return await base.SendAsync(request, ct);
        _logger.LogDebug("Adding session cookie to request");
        request.Headers.TryAddWithoutValidation("Cookie", cookie);

        return await base.SendAsync(request, ct);
    }
}
