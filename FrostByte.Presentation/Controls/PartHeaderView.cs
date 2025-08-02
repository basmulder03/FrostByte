namespace FrostByte.Presentation.Controls;

public partial class PartHeaderView : ContentView
{
    public static readonly BindableProperty PartNumberProperty =
        BindableProperty.Create(nameof(PartNumber), typeof(int), typeof(PartHeaderView),
            propertyChanged: OnPartNumberChanged);

    public PartHeaderView()
    {
        Content = new Label
        {
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            Margin = new Thickness(0, 10, 0, 5)
        };
    }

    public int PartNumber
    {
        get => (int)GetValue(PartNumberProperty);
        set => SetValue(PartNumberProperty, value);
    }

    private static void OnPartNumberChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (PartHeaderView)bindable;
        if (view.Content is Label label) label.Text = $"Part {newValue}";
    }
}
