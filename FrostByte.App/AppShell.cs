using FrostByte.Application.Services;
using FrostByte.Presentation.Views;

namespace FrostByte.App;

public partial class AppShell : Shell
{
    private readonly IAuthService _authService;

    public AppShell(CalendarPage calendarPage, AuthPage authPage, IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));

        var calendarPageShell = new ShellContent
        {
            ContentTemplate = new DataTemplate(() => calendarPage),
            Route = "CalendarPage"
        };
        SetNavBarIsVisible(calendarPageShell, false);
        Items.Add(calendarPageShell);

        _ = EnsureSignedInAsync(authPage);
    }

    private async Task EnsureSignedInAsync(AuthPage authPage)
    {
        if (!await _authService.IsAuthenticatedAsync())
        {
            // present AuthPage modally
            await Navigation.PushModalAsync(authPage);
        }
    }
}
