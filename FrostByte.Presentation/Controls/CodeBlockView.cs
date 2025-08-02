namespace FrostByte.Presentation.Controls;

public partial class CodeBlockView : ContentView
{
    public static readonly BindableProperty CodeProperty =
        BindableProperty.Create(nameof(Code), typeof(string), typeof(CodeBlockView),
            propertyChanged: OnCodeChanged);

    public CodeBlockView()
    {
        Content = new Border
        {
            HorizontalOptions = LayoutOptions.Start,
            Stroke = Colors.Gray,
            StrokeThickness = 1,
            Padding = 8,
            Content = new Label
            {
                FontFamily = "SourceCodeProRegular",
                FontSize = 14
            }
        };
    }

    public string Code
    {
        get => (string)GetValue(CodeProperty);
        set => SetValue(CodeProperty, value);
    }

    private static void OnCodeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (CodeBlockView)bindable;
        if (view.Content is Border border && border.Content is Label label) label.Text = (string)newValue;
    }
}
