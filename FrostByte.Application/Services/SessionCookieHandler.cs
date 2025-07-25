namespace FrostByte.Application.Services;

public class SessionCookieHandler(IAuthService authService) : DelegatingHandler
{
    private readonly IAuthService _authService = authService ?? throw new ArgumentNullException(nameof(authService));

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var cookie = await _authService.GetSessionCookieAsync();
        request.Headers.Remove("Cookie");
        if (!string.IsNullOrEmpty(cookie))
        {
            request.Headers.TryAddWithoutValidation("Cookie", cookie);
        }

        return await base.SendAsync(request, ct);
    }
}
