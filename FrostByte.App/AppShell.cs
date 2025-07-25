using FrostByte.Presentation.Views;

namespace FrostByte.App;

public partial class AppShell : Shell
{
    public AppShell(CalendarPage calendarPage)
    {
        var calendarPageShell = new ShellContent
        {
            ContentTemplate = new DataTemplate(() => calendarPage),
            Route = "CalendarPage"
        };
        SetNavBarIsVisible(calendarPageShell, false);
        Items.Add(calendarPageShell);
    }
}
