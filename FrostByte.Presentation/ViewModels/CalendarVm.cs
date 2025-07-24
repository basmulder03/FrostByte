using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrostByte.Application.Services;

namespace FrostByte.Presentation.ViewModels;

public record DayCell(int Day, bool IsUnlocked);

public partial class CalendarVm : ObservableObject
{
    private readonly ICalendarService calendarService;

    public CalendarVm(ICalendarService calendarService, TimeProvider timeProvider)
    {
        this.calendarService = calendarService;
        _year = timeProvider.GetUtcNow().Year;
        DayCells = [];
        RefreshCalendar();
    }

    [ObservableProperty] private int _year;

    public ObservableCollection<DayCell> DayCells { get; }

    [RelayCommand]
    private void PrevYear()
    {
        Year--;
        RefreshCalendar();
    }

    [RelayCommand]
    private void NextYear()
    {
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
        foreach (var day in calendarService.GetDays(Year))
        {
            var isUnlocked = calendarService.IsUnlocked(Year, day);
            DayCells.Add(new DayCell(day, isUnlocked));
        }
    }
}
