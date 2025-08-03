using FrostByte.Application.Models;
using Microsoft.Extensions.Logging;

namespace FrostByte.Presentation.Controls;

public partial class PuzzleView : ContentView
{
    public static readonly BindableProperty PuzzleProperty =
        BindableProperty.Create(nameof(Puzzle), typeof(PuzzleDto), typeof(PuzzleView),
            propertyChanged: OnPuzzleChanged);

    private readonly ILogger<PuzzleView> _logger;
    private readonly Func<PuzzlePartView> _puzzlePartViewFactory;

    private readonly VerticalStackLayout _stack;

    public PuzzleView(Func<PuzzlePartView> puzzlePartViewFactory, ILogger<PuzzleView> logger)
    {
        _puzzlePartViewFactory =
            puzzlePartViewFactory ?? throw new ArgumentNullException(nameof(puzzlePartViewFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _stack = new VerticalStackLayout
        {
            Spacing = 10,
            Padding = new Thickness(10),
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Fill
        };
        Content = new ScrollView
        {
            Content = _stack,
            Orientation = ScrollOrientation.Vertical,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
            VerticalScrollBarVisibility = ScrollBarVisibility.Default,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill
        };
    }

    public PuzzleDto Puzzle
    {
        get => (PuzzleDto)GetValue(PuzzleProperty);
        set => SetValue(PuzzleProperty, value);
    }

    private static void OnPuzzleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (PuzzleView)bindable;
        view.BuildPuzzle((PuzzleDto)newValue);
    }

    private void BuildPuzzle(PuzzleDto dto)
    {
        _stack.Children.Clear();

        // Add title
        var titleView = new PuzzleTitleView
        {
            Title = dto.Title
        };
        _stack.Add(titleView);

        // Add parts
        foreach (var part in dto.Parts)
        {
            var partView = _puzzlePartViewFactory();
            partView.Part = part;
            _stack.Add(partView);
        }
    }
}
