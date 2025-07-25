using FrostByte.Application.Services;
using FrostByte.Presentation.Views;

namespace FrostByte.App;

public partial class AppShell : Shell
{
    private readonly IAuthService _authService;

    public AppShell(Func<CalendarPage> calendarPageFactory, Func<DayPage> dayPageFactory, Func<AuthPage> authPageFactory, IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));

        AddPageToShell(calendarPageFactory, "CalendarPage", false);
        AddPageToShell(dayPageFactory, "DayPage", false);

        _ = EnsureSignedInAsync(authPageFactory);
    }

    private void AddPageToShell(Func<Page> pageFactory, string route, bool navBarVisible)
    {
        var shellContent = new ShellContent
        {
            ContentTemplate = new DataTemplate(pageFactory),
            Route = route
        };
        SetNavBarIsVisible(shellContent, navBarVisible);
        Items.Add(shellContent);
    }

    private async Task EnsureSignedInAsync(Func<AuthPage> authPageFactory)
    {
        if (!await _authService.IsAuthenticatedAsync())
        {
            // present AuthPage modally
            await Navigation.PushModalAsync(authPageFactory());
        }
    }
}
