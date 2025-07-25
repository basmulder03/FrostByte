using FrostByte.Presentation.Controls;
using FrostByte.Presentation.ViewModels;
using Microsoft.Extensions.Logging;

namespace FrostByte.Presentation.Views;

public partial class DayPage : ContentPage
{
    private readonly ILogger<DayPage> _logger;

    public DayPage(DayVm vm, Func<PuzzleView> puzzleViewFactory, ILogger<DayPage> logger)
    {
        BindingContext = vm;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var puzzleView = puzzleViewFactory();

        Content = new VerticalStackLayout
        {
            Spacing = 10,
            Padding = 10,
            Children = { puzzleView }
        };

        _logger.LogInformation("DayPage initialized for year {Year}, day {Day}", vm.Year, vm.Day);

        _ = vm.LoadCommand.ExecuteAsync(null);
        _logger.LogInformation("Puzzle loading initiated for year {Year}, day {Day}", vm.Year, vm.Day);
    }
}
