using FrostByte.Presentation.ViewModels;
using FrostByte.Presentation.Views;
using Microsoft.Extensions.Logging;

namespace FrostByte.App;

public partial class AppShell : Shell
{
    private readonly Func<Page> _authPageFactory;
    private readonly ILogger<AppShell> _logger;
    private readonly AppShellVm _vm;

    public AppShell(Func<CalendarPage> calendarPageFactory, Func<DayPage> dayPageFactory,
        Func<AuthPage> authPageFactory, AppShellVm vm, ILogger<AppShell> logger)
    {
        _vm = vm ?? throw new ArgumentNullException(nameof(vm));
        _authPageFactory = authPageFactory ?? throw new ArgumentNullException(nameof(authPageFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        FlyoutIsPresented = false;
        FlyoutBehavior = FlyoutBehavior.Disabled;
        AddPageToShell(calendarPageFactory, "CalendarPage", false);
        AddPageToShell(dayPageFactory, "DayPage", false);

        // Subscribe to the authentication required event
        _vm.AuthenticationRequired += OnAuthenticationRequired;
        Task.Run(async () => await _vm.CheckAuthenticationAsync());
    }

    private void AddPageToShell(Func<Page> pageFactory, string route, bool navBarVisible)
    {
        var shellContent = new ShellContent
        {
            ContentTemplate = new DataTemplate(pageFactory),
            Route = route
        };
        SetFlyoutItemIsVisible(shellContent, navBarVisible);
        SetNavBarIsVisible(shellContent, navBarVisible);
        Items.Add(shellContent);
    }

    private async void OnAuthenticationRequired(object? sender, EventArgs e)
    {
        try
        {
            await Navigation.PushModalAsync(_authPageFactory());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to navigate to authentication page.");
        }
    }
}
