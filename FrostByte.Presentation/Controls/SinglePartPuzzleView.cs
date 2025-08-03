using FrostByte.Application.Models;
using Microsoft.Extensions.Logging;

namespace FrostByte.Presentation.Controls;

public partial class SinglePartPuzzleView : ContentView
{
    public static readonly BindableProperty PuzzleProperty =
        BindableProperty.Create(nameof(Puzzle), typeof(PuzzleDto), typeof(SinglePartPuzzleView),
            propertyChanged: OnPuzzleChanged);

    public static readonly BindableProperty PartIndexProperty =
        BindableProperty.Create(nameof(PartIndex), typeof(int), typeof(SinglePartPuzzleView), 0,
            propertyChanged: OnPartIndexChanged);

    private readonly ILogger<SinglePartPuzzleView> _logger;
    private readonly Func<PuzzlePartView> _puzzlePartViewFactory;
    private readonly VerticalStackLayout _stack;

    public SinglePartPuzzleView(Func<PuzzlePartView> puzzlePartViewFactory, ILogger<SinglePartPuzzleView> logger)
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

    public int PartIndex
    {
        get => (int)GetValue(PartIndexProperty);
        set => SetValue(PartIndexProperty, value);
    }

    private static void OnPuzzleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (SinglePartPuzzleView)bindable;
        view.BuildPuzzlePart();
    }

    private static void OnPartIndexChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (SinglePartPuzzleView)bindable;
        view.BuildPuzzlePart();
    }

    private void BuildPuzzlePart()
    {
        _stack.Children.Clear();

        if (Puzzle == null || Puzzle.Parts == null || PartIndex >= Puzzle.Parts.Count || PartIndex < 0) return;

        // Add title
        var titleView = new PuzzleTitleView
        {
            Title = Puzzle.Title
        };
        _stack.Add(titleView);

        // Add only the selected part
        var part = Puzzle.Parts[PartIndex];
        var partView = _puzzlePartViewFactory();
        partView.Part = part;
        _stack.Add(partView);
    }
}
