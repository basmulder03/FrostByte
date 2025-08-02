using FrostByte.Presentation.Controls;
using FrostByte.Presentation.ViewModels;
using FrostByte.Presentation.Views;

namespace FrostByte.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFrostBytePresentation(this IServiceCollection services)
    {
        return services
                // Pages
                .AddTransient<CalendarPage>()
                .AddTransient<AuthPage>()
                .AddTransient<DayPage>()
                // Page Factories
                .AddTransient<Func<CalendarPage>>(sp => sp.GetRequiredService<CalendarPage>)
                .AddTransient<Func<AuthPage>>(sp => sp.GetRequiredService<AuthPage>)
                .AddTransient<Func<DayPage>>(sp => sp.GetRequiredService<DayPage>)
                // ViewModels
                .AddTransient<CalendarVm>()
                .AddTransient<DayVm>()
                // Controls
                .AddTransient<PuzzleView>()
                .AddTransient<Func<PuzzleView>>(sp => sp.GetRequiredService<PuzzleView>)
            ;
    }
}
