using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrostByte.Application.Configuration;
using FrostByte.Application.Services;

namespace FrostByte.Presentation.ViewModels;

public record DayCell(int Day, bool IsUnlocked);

public partial class CalendarVm : ObservableObject
{
    private readonly ICalendarService _calendarService;

    public CalendarVm(ICalendarService calendarService, TimeProvider timeProvider)
    {
        _calendarService = calendarService;
        _year = timeProvider.GetUtcNow().Year;
        DayCells = [];
        RefreshCalendar();
    }

    [ObservableProperty] private int _year;
    [ObservableProperty] private bool _hasPreviousYear;
    [ObservableProperty] private bool _hasNextYear;

    public ObservableCollection<DayCell> DayCells { get; }

    [RelayCommand]
    private void PrevYear()
    {
        if (!HasPreviousYear) return;
        Year--;
        RefreshCalendar();
    }

    [RelayCommand]
    private void NextYear()
    {
        if (!HasNextYear) return;
        Year++;
        RefreshCalendar();
    }

    [RelayCommand]
    private async Task OpenDayAsync(int day)
    {
        // Navigate to DayPage (ensure it accepts both year & day as parameters)
        await Shell.Current.GoToAsync(nameof(CalendarVm).Replace("Vm", "Page"),
            new Dictionary<string, object> { { "year", Year }, { "day", day } });
    }

    private void RefreshCalendar()
    {
        DayCells.Clear();
        foreach (var day in _calendarService.GetDays(Year))
        {
            var isUnlocked = _calendarService.IsUnlocked(Year, day);
            DayCells.Add(new DayCell(day, isUnlocked));
        }

        HasPreviousYear = _calendarService.YearAvailable(Year - 1);
        HasNextYear = _calendarService.YearAvailable(Year + 1);
    }
}
