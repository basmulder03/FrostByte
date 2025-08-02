using FrostByte.Application.Models;
using Microsoft.Extensions.Logging;

namespace FrostByte.Presentation.Controls;

public partial class PuzzleView : ContentView
{
    public static readonly BindableProperty PuzzleProperty =
        BindableProperty.Create(nameof(Puzzle), typeof(PuzzleDto), typeof(PuzzleView),
            propertyChanged: OnPuzzleChanged);

    private readonly ILogger<PuzzleView> _logger;

    private readonly VerticalStackLayout _stack;

    public PuzzleView(ILogger<PuzzleView> logger)
    {
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
                FontAttributes = FontAttributes.Bold
            });

            foreach (var block in part.Blocks)
                switch (block)
                {
                    case ParagraphBlock paragraph:
                        var paragraphLabel = new Label
                        {
                            FontSize = 16,
                            LineBreakMode = LineBreakMode.WordWrap
                        };

                        var formattedString = new FormattedString();
                        for (var i = 0; i < paragraph.Text.Count; i++)
                        {
                            var text = paragraph.Text[i];
                            var span = text switch
                            {
                                PlainText plain => new Span { Text = plain.Plain },
                                EmphasizedText emphasized => new Span
                                {
                                    Text = emphasized.Emphasized,
                                    FontAttributes = FontAttributes.Italic
                                },
                                StarEmphasizedText starEmphasized => new Span
                                {
                                    Text = starEmphasized.StarEmphasized,
                                    FontAttributes = FontAttributes.Italic,
                                    TextColor = Colors.Gold
                                },
                                CodeText code => new Span
                                {
                                    Text = $"\u200A{code.Code}\u200A", // Add thin spaces for padding
                                    FontFamily = "SourceCodeProRegular",
                                    BackgroundColor = Colors.LightGray,
                                    TextColor = Colors.DarkSlateGray
                                    // No border support in Span, but this matches code block style
                                },
                                EmphasizedCodeText emphasizedCode => new Span
                                {
                                    Text = $"\u200A{emphasizedCode.EmphasizedCode}\u200A",
                                    FontFamily = "SourceCodeProRegular",
                                    FontAttributes = FontAttributes.Italic,
                                    BackgroundColor = Colors.LightGray,
                                    TextColor = Colors.DarkSlateGray
                                },
                                TitleText titleText => new Span
                                {
                                    Text = titleText.Text,
                                    TextDecorations = TextDecorations.Underline
                                },
                                LinkText linkText => new Span
                                {
                                    Text = linkText.Text,
                                    TextDecorations = TextDecorations.Underline,
                                    GestureRecognizers =
                                    {
                                        new TapGestureRecognizer
                                        {
                                            Command = new Command(() =>
                                            {
                                                // For simplicity, we just log the link.
                                                _logger.LogInformation("Link tapped: {Url}", linkText.Url);
                                            })
                                        }
                                    },
                                    TextColor = Colors.Blue
                                },
                                _ => new Span { Text = "" }
                            };
                            formattedString.Spans.Add(span);
                            // Add a space between spans, except after the last one
                            if (i < paragraph.Text.Count - 1)
                                formattedString.Spans.Add(new Span { Text = " " });
                        }

                        paragraphLabel.FormattedText = formattedString;
                        _stack.Add(paragraphLabel);
                        break;
                    case CodeBlock codeBlock:
                        _stack.Add(new Border
                        {
                            HorizontalOptions = LayoutOptions.Start,
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
                        break;
                }
        }
    }
}
