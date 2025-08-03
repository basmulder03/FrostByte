using System.Collections.ObjectModel;
using System.ComponentModel;
using FrostByte.Presentation.Controls;
using FrostByte.Presentation.ViewModels;
using Microsoft.Extensions.Logging;

namespace FrostByte.Presentation.Views;

public partial class DayPage : ContentPage, IQueryAttributable
{
    private readonly ILogger<DayPage> _logger;
    private readonly Func<SinglePartPuzzleView> _singlePartPuzzleViewFactory;
    private readonly DayVm _vm;
    private ContentView _contentView = new();
    private ActivityIndicator _loadingIndicator = new();
    private Grid _mainGrid = new();

    private int _selectedTabIndex;
    private TabBarView _tabBar = new();

    public DayPage(DayVm vm, Func<SinglePartPuzzleView> singlePartPuzzleViewFactory, ILogger<DayPage> logger)
    {
        _vm = vm;
        BindingContext = vm;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _singlePartPuzzleViewFactory = singlePartPuzzleViewFactory;
        _logger.LogInformation("Initializing DayPage for year {Year}, day {Day}", vm.Year, vm.Day);

        // Subscribe to property changes to rebuild tabs when data loads
        _vm.PropertyChanged += OnViewModelPropertyChanged;

        InitializeContent();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Year", out var yearObj) && yearObj is int year) _vm.Year = year;
        if (query.TryGetValue("Day", out var dayObj) && dayObj is int day) _vm.Day = day;
        _logger.LogInformation("ApplyQueryAttributes: Year={Year}, Day={Day}", _vm.Year, _vm.Day);
        InitializeContent();
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DayVm.IsLoading))
        {
            UpdateLoadingState();
        }
        else if (e.PropertyName == nameof(DayVm.Puzzle) && _vm.Puzzle != null)
        {
            // Rebuild tabs when puzzle data becomes available
            BuildTabs();
            UpdateContentView();
        }
    }

    private void OnCloseClicked(object? sender, EventArgs e)
    {
        // Navigate to CalendarPage for the current year
        var year = _vm.Year;
        Shell.Current.GoToAsync($"//CalendarPage?Year={year}");
    }

    private void InitializeContent()
    {
        if (_vm.Year == 0 || _vm.Day == 0)
        {
            _logger.LogInformation("DayPage is still loading, waiting for Year and Day to be set.");
            ShowLoading();
            return;
        }

        SetupMainLayout();

        // Start loading if we have Year and Day
        if (_vm.Year > 0 && _vm.Day > 0)
        {
            _ = _vm.LoadCommand.ExecuteAsync(null);
            _logger.LogInformation("Puzzle loading initiated for year {Year}, day {Day}", _vm.Year, _vm.Day);
        }
    }

    private void SetupMainLayout()
    {
        // Initialize the loading indicator
        _loadingIndicator = new ActivityIndicator
        {
            IsRunning = true,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Color = Colors.DodgerBlue,
            Scale = 1.5
        };

        // Initialize tab bar
        _tabBar = new TabBarView
        {
            Tabs = new ObservableCollection<TabItem>(),
            IsVisible = false // Hide initially until data loads
        };
        _tabBar.TabSelectedCommand = new Command<int>(OnTabSelected);

        // Content area
        _contentView = new ContentView
        {
            IsVisible = false // Hide initially until data loads
        };

        // Close button overlay
        var closeButton = new Button
        {
            Text = "✕",
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Black,
            FontSize = 28,
            Padding = new Thickness(10),
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start,
            ZIndex = 1000
        };
        closeButton.Clicked += OnCloseClicked;

        // Setup main grid
        _mainGrid = new Grid();
        _mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        _mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

        // Add components to grid
        _mainGrid.Children.Add(_tabBar);
        Grid.SetRow(_tabBar, 0);

        _mainGrid.Children.Add(_contentView);
        Grid.SetRow(_contentView, 1);

        _mainGrid.Children.Add(_loadingIndicator);
        Grid.SetRow(_loadingIndicator, 1);

        _mainGrid.Children.Add(closeButton);

        Content = _mainGrid;

        // Show loading state initially
        UpdateLoadingState();
    }

    private void ShowLoading()
    {
        Content = new ActivityIndicator
        {
            IsRunning = true,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
    }

    private void UpdateLoadingState()
    {
        if (_vm.IsLoading)
        {
            _loadingIndicator.IsVisible = true;
            _loadingIndicator.IsRunning = true;
            _tabBar.IsVisible = false;
            _contentView.IsVisible = false;
        }
        else
        {
            _loadingIndicator.IsVisible = false;
            _loadingIndicator.IsRunning = false;
            _tabBar.IsVisible = true;
            _contentView.IsVisible = true;
        }
    }

    private void BuildTabs()
    {
        var tabs = new ObservableCollection<TabItem>();
        var puzzle = _vm.Puzzle;
        var partCount = puzzle?.Parts?.Count ?? 0;

        // Add part tabs
        for (var i = 0; i < partCount; i++)
            tabs.Add(new TabItem
            {
                Title = $"Part {i + 1}",
                Data = i
            });

        // Add input tab
        tabs.Add(new TabItem
        {
            Title = "Input",
            Data = "input"
        });

        _tabBar.Tabs = tabs;
        _tabBar.SelectedIndex = _selectedTabIndex;

        _logger.LogInformation("Built {TabCount} tabs for puzzle with {PartCount} parts", tabs.Count, partCount);
    }

    private void OnTabSelected(int index)
    {
        _selectedTabIndex = index;
        UpdateContentView();
    }

    private void UpdateContentView()
    {
        var puzzle = _vm.Puzzle;
        var partCount = puzzle?.Parts?.Count ?? 0;

        if (_selectedTabIndex < partCount)
        {
            // Show specific puzzle part
            var puzzleView = _singlePartPuzzleViewFactory();
            puzzleView.BindingContext = _vm;
            puzzleView.SetBinding(SinglePartPuzzleView.PuzzleProperty, nameof(DayVm.Puzzle));
            puzzleView.PartIndex = _selectedTabIndex;
            _contentView.Content = puzzleView;
        }
        else
        {
            // Show puzzle input
            _contentView.Content = new ScrollView
            {
                Content = new Label
                {
                    Text = _vm.PuzzleInput ?? string.Empty,
                    FontSize = 14,
                    Padding = 10,
                    BackgroundColor = Color.FromArgb("#1E1E1E"), // Dark background for input
                    TextColor = Color.FromArgb("#D4D4D4") // Light text color
                }
            };
        }
    }
}
