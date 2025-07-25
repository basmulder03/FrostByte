using FrostByte.Presentation.ViewModels;
using Microsoft.Extensions.Logging;

namespace FrostByte.Presentation.Views;

public partial class DayPage : ContentPage
{
    private readonly ILogger<DayPage> _logger;

    public DayPage(DayVm vm, ILogger<DayPage> logger)
    {
        BindingContext = vm;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _logger.LogInformation("DayPage initialized for year {Year}, day {Day}", vm.Year, vm.Day);
    }
}
