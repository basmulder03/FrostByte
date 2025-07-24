using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using FrostByte.App.Extendsions;
using FrostByte.Application.Configuration;
using FrostByte.Application.Services;
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

        // Load settings from the appdata settings.json file
        IConfiguration config = new ConfigurationBuilder().Build();

        // Application Settings
        builder.Services.Configure<WorkbenchSettings>(config);

        // Core Services
        builder.Services.AddSingleton<ICalendarService, CalendarService>();

        // Presentation
        builder.Services.AddTransient<CalendarVm>();
        builder.Services.AddTransient<CalendarPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
