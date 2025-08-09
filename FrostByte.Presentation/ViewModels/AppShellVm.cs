using FrostByte.Application.Services;

namespace FrostByte.Presentation.ViewModels;

/// <summary>
///     View model for the application shell. It encapsulates authentication
///     checks so that the UI does not directly depend on service implementations.
///     When authentication is required, the <see cref="AuthenticationRequired" />
///     event will be raised.
/// </summary>
public class AppShellVm(IAuthService authService)
{
    /// <summary>
    ///     Raised when the user is not authenticated and the UI should present
    ///     an authentication page.
    /// </summary>
    public event EventHandler? AuthenticationRequired;

    /// <summary>
    ///     Checks whether the user is authenticated. If not, the
    ///     <see cref="AuthenticationRequired" /> event is raised.
    /// </summary>
    public async Task CheckAuthenticationAsync()
    {
        var isAuthenticated = await authService.IsAuthenticatedAsync();
        if (!isAuthenticated) AuthenticationRequired?.Invoke(this, EventArgs.Empty);
    }
}
