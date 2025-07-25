using FrostByte.Application.Configuration;
using FrostByte.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FrostByte.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFrostByteApplication(this IServiceCollection services)
    {
        return services
            // Services
            .AddSingleton<ISettingsService, SettingService>()
            .AddSingleton<ICalendarService, CalendarService>()

            // Add TimeProvider
            .AddSingleton(TimeProvider.System)

            // load settings once, and cache
            .AddSingleton<WorkbenchSettings>(sp =>
            {
                var settingsSvc = sp.GetRequiredService<ISettingsService>();
                // Be sure to use ConfigureAwait(false) to avoid deadlocks
                return settingsSvc.LoadAsync()
                    .AsTask()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            });
    }
}
