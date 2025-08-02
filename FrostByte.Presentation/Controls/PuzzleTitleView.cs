namespace FrostByte.Presentation.Controls;

public partial class PuzzleTitleView : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(PuzzleTitleView),
            propertyChanged: OnTitleChanged);

    public PuzzleTitleView()
    {
        Content = new Label
        {
            FontSize = 28,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (PuzzleTitleView)bindable;
        if (view.Content is Label label) label.Text = (string)newValue;
    }
}
