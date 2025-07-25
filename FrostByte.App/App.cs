namespace FrostByte.App;

public partial class App : Microsoft.Maui.Controls.Application
{
    private readonly AppShell _appShell;

    public App(AppShell appShell)
    {
        _appShell = appShell;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_appShell);
    }
}
