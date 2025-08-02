using FrostByte.Application.Models;

namespace FrostByte.Presentation.Controls;

public partial class PuzzlePartView : ContentView
{
    public static readonly BindableProperty PartProperty =
        BindableProperty.Create(nameof(Part), typeof(PartDto), typeof(PuzzlePartView),
            propertyChanged: OnPartChanged);

    private readonly VerticalStackLayout _stack;

    public PuzzlePartView()
    {
        _stack = new VerticalStackLayout
        {
            Spacing = 5
        };

        Content = _stack;
    }

    public PartDto Part
    {
        get => (PartDto)GetValue(PartProperty);
        set => SetValue(PartProperty, value);
    }

    private static void OnPartChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (PuzzlePartView)bindable;
        view.BuildPart((PartDto)newValue);
    }

    private void BuildPart(PartDto part)
    {
        _stack.Children.Clear();

        // Add part header
        var partHeader = new PartHeaderView
        {
            PartNumber = part.PartNumber
        };
        _stack.Add(partHeader);

        // Add blocks
        foreach (var block in part.Blocks)
            switch (block)
            {
                case ParagraphBlock paragraph:
                    var formattedText = new FormattedTextView
                    {
                        TextElements = paragraph.Text
                    };
                    _stack.Add(formattedText);
                    break;

                case CodeBlock codeBlock:
                    var codeBlockView = new CodeBlockView
                    {
                        Code = codeBlock.Code
                    };
                    _stack.Add(codeBlockView);
                    break;
            }
    }
}
