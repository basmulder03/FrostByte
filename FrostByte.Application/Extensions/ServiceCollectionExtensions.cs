using FrostByte.Application.Configuration;
using FrostByte.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FrostByte.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFrostByteApplication(this IServiceCollection services)
    {
        services
            // Services
            .AddSingleton<ISettingsService, SettingService>()
            .AddSingleton<ICalendarService, CalendarService>()
            .AddSingleton<IAuthService, AuthService>()

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

        // AoC HTTP Client with dynamic cookie injection and compliance with AoC automation rules
        const string baseUrl = "https://adventofcode.com/";
        const string userAgent = "github.com/basmulder03/FrostByte by bas@basmulder.online";
        services.AddTransient<SessionCookieHandler>()
            .AddHttpClient("AoC", c =>
            {
                c.BaseAddress = new Uri(baseUrl);
                c.DefaultRequestHeaders.Add("User-Agent", userAgent);
            })
            .AddHttpMessageHandler<SessionCookieHandler>();

        return services;
    }
}
