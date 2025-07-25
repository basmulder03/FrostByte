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

                // ViewModels
                .AddTransient<CalendarVm>()
            ;
    }
}
