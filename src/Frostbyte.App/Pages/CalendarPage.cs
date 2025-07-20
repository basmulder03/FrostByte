namespace Frostbyte.App.Pages;

public class CalendarPage : ContentPage
{
    public CalendarPage()
    {
        Content = new VerticalStackLayout
        {
            new Label
            {
                Text = "Welcome to Frostbyte ❄️",
                FontSize = 32,
                HorizontalOptions = LayoutOptions.Center
            }
        };
    }
}