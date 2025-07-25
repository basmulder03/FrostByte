using FrostByte.Application.Models;

namespace FrostByte.Presentation.Controls;

public partial class PuzzleView : ContentView
{
    private static readonly BindableProperty PuzzleProperty =
        BindableProperty.Create(nameof(Puzzle), typeof(PuzzleDto), typeof(PuzzleView), propertyChanged: OnPuzzleChanged);

    public PuzzleDto Puzzle
    {
        get => (PuzzleDto)GetValue(PuzzleProperty);
        set => SetValue(PuzzleProperty, value);
    }

    private readonly VerticalStackLayout _stack;

    public PuzzleView()
    {
        _stack = new VerticalStackLayout
        {
            Spacing = 10,
            Padding = new Thickness(10)
        };
        Content = new ScrollView
        {
            Content = _stack
        };
    }

    private static void OnPuzzleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (PuzzleView)bindable;
        view.BuildPuzzle((PuzzleDto)newValue);
    }

    private void BuildPuzzle(PuzzleDto dto)
    {
        _stack.Children.Clear();

        // Title
        _stack.Add(new Label
        {
            Text = dto.Title,
            FontSize = 28,
            FontAttributes = FontAttributes.Bold
        });

        // Parts (usually 1 & 2)
        foreach (var part in dto.Parts)
        {
            _stack.Add(new Label
            {
                Text = $"Part {part.PartNumber}",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
            });

            foreach (var block in part.Blocks)
            {
                if (block is ParagraphBlock paragraphBlock)
                {
                    _stack.Add(new Label
                    {
                        Text = paragraphBlock.Text,
                        LineBreakMode = LineBreakMode.WordWrap
                    });
                }
                else if (block is CodeBlock codeBlock)
                {
                    _stack.Add(new Border
                    {
                        Stroke = Colors.Gray,
                        StrokeThickness = 1,
                        Padding = 8,
                        Content = new Label
                        {
                            Text = codeBlock.Code,
                            FontFamily = "SourceCodeProRegular",
                            FontSize = 14
                        }
                    });
                }
            }
        }
    }
}
