using FrostByte.Application.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FrostByte.Application.Services;

public class CalendarService : ICalendarService
{
    private readonly WorkbenchSettings _settings;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<CalendarService> _logger;

    public CalendarService(WorkbenchSettings workbenchSettings, TimeProvider timeProvider, ILogger<CalendarService> logger)
    {
        _settings = workbenchSettings;
        _timeProvider = timeProvider;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _logger.LogInformation("CalendarService initialized with FirstPuzzleYear: {FirstPuzzleYear}, DailyPuzzleOpenTime: {DailyPuzzleOpenTime}",
            _settings.FirstPuzzleYear, _settings.DailyPuzzleOpenTime);
    }

    public IEnumerable<int> GetDays(int year)
    {
        _logger.LogDebug("Getting days for year {Year}", year);
        for (var day = 1; day <= 25; day++)
            yield return day;
    }

    public bool IsUnlocked(int year, int day)
    {
        var openUtc = new DateTimeOffset(
            year,
            12,
            day,
            _settings.DailyPuzzleOpenTime.Hour,
            _settings.DailyPuzzleOpenTime.Minute,
            0,
            TimeSpan.Zero
        );

        _logger.LogDebug("Checking if day {Day} of year {Year} is unlocked. Current UTC time: {CurrentUtcTime}, Open time: {OpenUtcTime}",
            day, year, _timeProvider.GetUtcNow(), openUtc);
        return _timeProvider.GetUtcNow() >= openUtc;
    }

    public bool YearAvailable(int year)
    {
        _logger.LogDebug("Checking if year {Year} is available. First puzzle year: {FirstPuzzleYear}, Current year: {CurrentYear}",
            year, _settings.FirstPuzzleYear, _timeProvider.GetUtcNow().Year);
        return year >= _settings.FirstPuzzleYear
               && year <= _timeProvider.GetUtcNow().Year;
    }
}
