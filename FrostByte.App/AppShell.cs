namespace FrostByte.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        var shellContent = new ShellContent
        {
            ContentTemplate = new DataTemplate(() => new MainPage()),
            Route = "MainPage"
        };
        SetNavBarIsVisible(shellContent, false);
        Items.Add(shellContent);
    }
}
