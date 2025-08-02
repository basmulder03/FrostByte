using FrostByte.Presentation.Controls;
using FrostByte.Presentation.ViewModels;
using Microsoft.Extensions.Logging;

namespace FrostByte.Presentation.Views;

public partial class DayPage : ContentPage, IQueryAttributable
{
    private readonly ILogger<DayPage> _logger;

    private readonly Func<PuzzleView> _puzzleViewFactory;
    private readonly DayVm _vm;

    public DayPage(DayVm vm, Func<PuzzleView> puzzleViewFactory, ILogger<DayPage> logger)
    {
        _vm = vm;
        BindingContext = vm;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _puzzleViewFactory = puzzleViewFactory;
        _logger.LogInformation("Initializing DayPage for year {Year}, day {Day}", vm.Year, vm.Day);

        InitializeContent();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Year", out var yearObj) && yearObj is int year) _vm.Year = year;
        if (query.TryGetValue("Day", out var dayObj) && dayObj is int day) _vm.Day = day;
        _logger.LogInformation("ApplyQueryAttributes: Year={Year}, Day={Day}", _vm.Year, _vm.Day);
        InitializeContent();
    }

    private void InitializeContent()
    {
        if (_vm.Year == 0 || _vm.Day == 0)
        {
            _logger.LogInformation("DayPage is still loading, waiting for Year and Day to be set.");
            Content = new ActivityIndicator();
            return;
        }

        var puzzleView = _puzzleViewFactory();
        puzzleView.BindingContext = _vm;
        puzzleView.SetBinding(PuzzleView.PuzzleProperty, nameof(DayVm.Puzzle));

        // Set the PuzzleView directly as Content instead of wrapping it in a VerticalStackLayout
        // This allows the ScrollView inside PuzzleView to properly determine its constraints
        Content = puzzleView;

        _logger.LogInformation("DayPage initialized for year {Year}, day {Day}", _vm.Year, _vm.Day);

        _ = _vm.LoadCommand.ExecuteAsync(null);
        _logger.LogInformation("Puzzle loading initiated for year {Year}, day {Day}", _vm.Year, _vm.Day);
    }
}
