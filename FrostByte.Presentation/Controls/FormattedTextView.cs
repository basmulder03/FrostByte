using FrostByte.Application.Models;
using Microsoft.Extensions.Logging;

namespace FrostByte.Presentation.Controls;

public partial class FormattedTextView : ContentView
{
    public static readonly BindableProperty TextElementsProperty =
        BindableProperty.Create(nameof(TextElements), typeof(IList<Text>), typeof(FormattedTextView),
            propertyChanged: OnTextElementsChanged);

    private readonly ILogger<FormattedTextView> _logger;

    public FormattedTextView(ILogger<FormattedTextView> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Content = new Label
        {
            FontSize = 16,
            LineBreakMode = LineBreakMode.WordWrap
        };
    }

    public IList<Text> TextElements
    {
        get => (IList<Text>)GetValue(TextElementsProperty);
        set => SetValue(TextElementsProperty, value);
    }

    private static void OnTextElementsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (FormattedTextView)bindable;
        view.BuildFormattedText((IList<Text>)newValue);
    }

    private void BuildFormattedText(IList<Text> textElements)
    {
        if (Content is not Label label || textElements == null)
            return;

        var formattedString = new FormattedString();

        for (var i = 0; i < textElements.Count; i++)
        {
            var text = textElements[i];
            var span = CreateSpanForText(text);
            formattedString.Spans.Add(span);

            // Add a space between spans, except after the last one
            if (i < textElements.Count - 1)
                formattedString.Spans.Add(new Span { Text = " " });
        }

        label.FormattedText = formattedString;
    }

    private Span CreateSpanForText(Text text)
    {
        return text switch
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
                        Command = new Command(() => { _logger.LogInformation("Link tapped: {Url}", linkText.Url); })
                    }
                },
                TextColor = Colors.Blue
            },
            _ => new Span { Text = "" }
        };
    }
}
