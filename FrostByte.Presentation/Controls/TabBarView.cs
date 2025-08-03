using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FrostByte.Presentation.Controls;

public partial class TabBarView : ContentView
{
    public static readonly BindableProperty TabsProperty =
        BindableProperty.Create(nameof(Tabs), typeof(ObservableCollection<TabItem>), typeof(TabBarView),
            new ObservableCollection<TabItem>(), propertyChanged: OnTabsChanged);

    public static readonly BindableProperty SelectedIndexProperty =
        BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(TabBarView), 0,
            BindingMode.TwoWay, propertyChanged: OnSelectedIndexChanged);

    public static readonly BindableProperty TabSelectedCommandProperty =
        BindableProperty.Create(nameof(TabSelectedCommand), typeof(ICommand), typeof(TabBarView));

    private readonly List<Button> _tabButtons = [];

    private readonly StackLayout _tabContainer;

    public TabBarView()
    {
        _tabContainer = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = 0,
            HorizontalOptions = LayoutOptions.Fill,
            BackgroundColor = Color.FromArgb("#2D2D30"), // VS Code dark tab bar background
            Padding = new Thickness(0)
        };

        Content = _tabContainer;
    }

    public ObservableCollection<TabItem> Tabs
    {
        get => (ObservableCollection<TabItem>)GetValue(TabsProperty);
        set => SetValue(TabsProperty, value);
    }

    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public ICommand TabSelectedCommand
    {
        get => (ICommand)GetValue(TabSelectedCommandProperty);
        set => SetValue(TabSelectedCommandProperty, value);
    }

    private static void OnTabsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (TabBarView)bindable;
        view.BuildTabs();
    }

    private static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (TabBarView)bindable;
        view.UpdateSelectedTab();
    }

    private void BuildTabs()
    {
        _tabButtons.Clear();
        _tabContainer.Children.Clear();

        if (Tabs == null) return;

        for (var i = 0; i < Tabs.Count; i++)
        {
            var index = i;
            var tab = Tabs[i];
            var isSelected = index == SelectedIndex;

            var button = new Button
            {
                Text = tab.Title,
                FontSize = 13,
                Padding = new Thickness(8, 8),
                BorderWidth = 0,
                CornerRadius = 0,
                TextColor = isSelected ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#CCCCCC"),
                BackgroundColor = isSelected ? Color.FromArgb("#1E1E1E") : Color.FromArgb("#2D2D30"),
                HorizontalOptions = LayoutOptions.Fill
            };

            button.Clicked += (_, _) => OnTabClicked(index);
            _tabButtons.Add(button);
            _tabContainer.Children.Add(button);

            // Add a subtle separator between tabs (except for the last one)
            if (i < Tabs.Count - 1)
            {
                var separator = new BoxView
                {
                    Color = Color.FromArgb("#3E3E42"),
                    WidthRequest = 1,
                    VerticalOptions = LayoutOptions.Fill
                };
                _tabContainer.Children.Add(separator);
            }
        }
    }

    private void OnTabClicked(int index)
    {
        SelectedIndex = index;
        TabSelectedCommand?.Execute(index);
    }

    private void UpdateSelectedTab()
    {
        for (var i = 0; i < _tabButtons.Count; i++)
        {
            var button = _tabButtons[i];
            var isSelected = i == SelectedIndex;

            button.TextColor = isSelected ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#CCCCCC");
            button.BackgroundColor = isSelected ? Color.FromArgb("#1E1E1E") : Color.FromArgb("#2D2D30");
        }
    }
}

public class TabItem
{
    public string Title { get; set; } = string.Empty;
    public object? Data { get; set; }
}
