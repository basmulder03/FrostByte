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
                .AddTransient<SinglePartPuzzleView>()
                .AddTransient<TabBarView>()
                .AddTransient<CodeBlockView>()
                .AddTransient<PartHeaderView>()
                .AddTransient<PuzzleTitleView>()
                .AddTransient<FormattedTextView>()
                .AddTransient<PuzzlePartView>()
                // View Factories
                .AddTransient<Func<PuzzleView>>(sp => sp.GetRequiredService<PuzzleView>)
                .AddTransient<Func<SinglePartPuzzleView>>(sp => sp.GetRequiredService<SinglePartPuzzleView>)
                .AddTransient<Func<CodeBlockView>>(sp => sp.GetRequiredService<CodeBlockView>)
                .AddTransient<Func<PartHeaderView>>(sp => sp.GetRequiredService<PartHeaderView>)
                .AddTransient<Func<PuzzleTitleView>>(sp => sp.GetRequiredService<PuzzleTitleView>)
                .AddTransient<Func<FormattedTextView>>(sp => sp.GetRequiredService<FormattedTextView>)
                .AddTransient<Func<PuzzlePartView>>(sp => sp.GetRequiredService<PuzzlePartView>)
            ;
    }
}
