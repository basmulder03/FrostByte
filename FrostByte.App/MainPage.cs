namespace FrostByte.App;

using CommunityToolkit.Maui.Markup;

public partial class MainPage : ContentPage
{
    private int _count;
    private readonly Button counterBtn;

    public MainPage()
    {
        counterBtn = new Button
        {
            Text = "Click me",
            HorizontalOptions = LayoutOptions.Fill
        };

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Padding = new Thickness(30, 0),
                Spacing = 25,
                Children =
                {
                    new Image()
                        .Source("dotnet_bot.png")
                        .Height(185)
                        .Aspect(Aspect.AspectFit)
                        .SemanticDescription("dot net bot in a hovercraft number nine"),

                    new Label()
                        .Text("Hello, World!")
                        .SemanticHeadingLevel(SemanticHeadingLevel.Level1),

                    new Label()
                        .Text("Welcome to \n.NET Multi-platform App UI")
                        .SemanticHeadingLevel(SemanticHeadingLevel.Level2)
                        .SemanticDescription("Welcome to dot net Multi platform App U I"),

                    counterBtn
                }
            }
        };

        counterBtn.Clicked += OnCounterClicked;
    }

    private void OnCounterClicked(object? sender, EventArgs e)
    {
        _count++;
        counterBtn.Text = _count == 1 ? $"Clicked {_count} time" : $"Clicked {_count} times";
        SemanticScreenReader.Announce(counterBtn.Text);
    }
}
