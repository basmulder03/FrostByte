using FrostByte.Presentation.ViewModels;
using CommunityToolkit.Maui.Markup;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;
using Microsoft.Maui.Controls;

namespace FrostByte.Presentation.Views;

public class CalendarPage : ContentPage
{
    public CalendarPage(CalendarVm vm)
    {
        BindingContext = vm;

        // Header: < 2025 >
        var header = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 20,
            Children =
            {
                new Button()
                    .Text("<")
                    .Bind(Button.CommandProperty, nameof(vm.PrevYearCommand))
                    .Bind(IsEnabledProperty, nameof(vm.HasPreviousYear)),
                new Label()
                    .Bind(Label.TextProperty, nameof(vm.Year))
                    .FontSize(24)
                    .CenterVertical(),
                new Button()
                    .Text(">")
                    .Bind(Button.CommandProperty, nameof(vm.NextYearCommand))
                    .Bind(IsEnabledProperty, nameof(vm.HasNextYear))
            }
        };

        // 5×5 grid
        var grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Star }
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            Margin = 10,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };


        // Populate grid with day cells
        vm.DayCells.CollectionChanged += (_, _) => PopulateGrid(grid, vm);
        PopulateGrid(grid, vm);

        Content = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto }, // Header
                new RowDefinition { Height = GridLength.Star } // Calendar grid fills remaining space
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star }
            },
            Children =
            {
                header.Row(0).Column(0), // Header in first row
                grid.Row(1).Column(0) // Calendar grid in second row
            }
        };
    }

    private static void PopulateGrid(Grid grid, CalendarVm vm)
    {
        grid.Children.Clear();
        const int gridSize = 5;
        for (var i = 0; i < vm.DayCells.Count; i++)
        {
            var cell = vm.DayCells[i];
            int row = i / gridSize, col = i % gridSize;

            var btn = new Button
            {
                Text = cell.Day.ToString(),
                BackgroundColor = Colors.Transparent,
                BorderColor = Colors.Gray,
                BorderWidth = 1,
                IsEnabled = cell.IsUnlocked,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            btn.SetBinding(Button.CommandProperty, new Binding(nameof(CalendarVm.OpenDayCommand), source: vm));
            btn.SetBinding(Button.CommandParameterProperty, new Binding(nameof(cell.Day), source: cell));
            btn.CornerRadius = 0;

            grid.Add(btn, col, row);
        }
    }
}
