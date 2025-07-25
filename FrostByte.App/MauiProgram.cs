using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using FrostByte.App.Extendsions;
using FrostByte.Application.Configuration;
using FrostByte.Application.Extensions;
using FrostByte.Application.Services;
using FrostByte.Presentation.Extensions;
using FrostByte.Presentation.ViewModels;
using FrostByte.Presentation.Views;
using Microsoft.Extensions.Configuration;
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

        // Core Services
        builder.Services.AddFrostByteApplication();

        // Presentation
        builder.Services.AddFrostBytePresentation();

        // App
        builder.Services.AddSingleton<AppShell>();

#if WINDOWS
        builder.Services.AddSingleton<ISecretStore, WindowsSecretStore>();
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
