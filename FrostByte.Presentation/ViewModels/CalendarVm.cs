using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrostByte.Application.Configuration;
using FrostByte.Application.Services;
using Microsoft.Extensions.Logging;

namespace FrostByte.Presentation.ViewModels;

public record DayCell(int Day, bool IsUnlocked);

public partial class CalendarVm : ObservableObject
{
    private readonly ICalendarService _calendarService;
    private readonly ILogger<CalendarVm> _logger;

    public CalendarVm(ICalendarService calendarService, TimeProvider timeProvider, ILogger<CalendarVm> logger)
    {
        _calendarService = calendarService;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _year = timeProvider.GetUtcNow().Year;
        DayCells = [];
        RefreshCalendar();
        _logger.LogInformation("CalendarVm initialized for year {Year}", _year);
    }

    public int Year
    {
        get => _year;
        set => SetProperty(ref _year, value);
    }

    private int _year;

    public bool HasPreviousYear
    {
        get => _hasPreviousYear;
        set => SetProperty(ref _hasPreviousYear, value);
    }

    private bool _hasPreviousYear;

    public bool HasNextYear
    {
        get => _hasNextYear;
        set => SetProperty(ref _hasNextYear, value);
    }

    private bool _hasNextYear;

    public ObservableCollection<DayCell> DayCells { get; }

    [RelayCommand]
    private void PrevYear()
    {
        _logger.LogDebug("Moving to previous year: {Year}", Year - 1);
        if (!HasPreviousYear) return;
        Year--;
        RefreshCalendar();
    }

    [RelayCommand]
    private void NextYear()
    {
        _logger.LogDebug("Moving to next year: {Year}", Year + 1);
        if (!HasNextYear) return;
        Year++;
        RefreshCalendar();
    }

    [RelayCommand]
    private async Task OpenDayAsync(int day)
    {
        _logger.LogDebug("Opening day {Day} for year {Year}", day, Year);
        // Navigate to DayPage (ensure it accepts both year & day as parameters)
        await Shell.Current.GoToAsync(nameof(CalendarVm).Replace("Vm", "Page"),
            new Dictionary<string, object> { { "year", Year }, { "day", day } });
    }

    private void RefreshCalendar()
    {
        _logger.LogDebug("Refreshing calendar for year {Year}", Year);
        DayCells.Clear();
        foreach (var day in _calendarService.GetDays(Year))
        {
            var isUnlocked = _calendarService.IsUnlocked(Year, day);
            DayCells.Add(new DayCell(day, isUnlocked));
        }

        HasPreviousYear = _calendarService.YearAvailable(Year - 1);
        HasNextYear = _calendarService.YearAvailable(Year + 1);
        _logger.LogInformation("Calendar refreshed for year {Year} with {DayCount} days", Year, DayCells.Count);
    }
}
