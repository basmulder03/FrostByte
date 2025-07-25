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
            RowDefinitions = Rows.Define(5, GridLength.Auto),
            ColumnDefinitions = Columns.Define(5, GridLength.Auto),
            Margin = 10,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };


        // Populate grid with day cells
        vm.DayCells.CollectionChanged += (_, _) => PopulateGrid(grid, vm);
        PopulateGrid(grid, vm);

        Content = new VerticalStackLayout
        {
            Spacing = 10,
            Padding = 10,
            Children = { header, grid }
        };
    }

    private static void PopulateGrid(Grid grid, CalendarVm vm)
    {
        grid.Children.Clear();
        for (var i = 0; i < vm.DayCells.Count; i++)
        {
            var cell = vm.DayCells[i];
            int row = i / 5, col = i % 5;

            var btn = new Button()
                .Text(cell.Day.ToString())
                .Bind(Button.CommandProperty, nameof(CalendarVm.OpenDayCommand), source: vm)
                .Bind(Button.CommandParameterProperty, nameof(cell.Day), source: cell)
                .Center();
            btn.IsEnabled = cell.IsUnlocked;
            btn.Opacity = cell.IsUnlocked ? 1 : 0.4;

            grid.Add(btn, col, row);
        }
    }
}
