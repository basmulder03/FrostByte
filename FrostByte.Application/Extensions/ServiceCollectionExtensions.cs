using FrostByte.Application.Clients;
using FrostByte.Application.Configuration;
using FrostByte.Application.Parsing;
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
            .AddSingleton<IDayService, DayService>()

            // Transformers
            .AddSingleton<IPuzzleTransformer, PuzzleTransformer>()

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

        // Register AdventOfCodeHttpClient as a typed client
        const string baseUrl = "https://adventofcode.com/";
        const string userAgent = "FrostByte/1.0 (+https://github.com/basmulder03/FrostByte; bas@basmulder.online)";
        services.AddTransient<SessionCookieHandler>()
            .AddHttpClient<IAdventOfCodeHttpClient, AdventOfCodeHttpClient>(c =>
            {
                c.BaseAddress = new Uri(baseUrl);
                c.DefaultRequestHeaders.Add("User-Agent", userAgent);
            })
            .AddHttpMessageHandler<SessionCookieHandler>();

        return services;
    }
}
