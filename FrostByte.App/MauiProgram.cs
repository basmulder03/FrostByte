using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using FrostByte.App.Extendsions;
using Microsoft.Extensions.Logging;

namespace FrostByte.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup()
            .ConfigureFonts(fonts => { fonts.AddSourceCodeProFont(); });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
