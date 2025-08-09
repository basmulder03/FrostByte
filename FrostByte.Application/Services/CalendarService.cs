using FrostByte.Application.Configuration;
using Microsoft.Extensions.Logging;

namespace FrostByte.Application.Services;

public class CalendarService : ICalendarService
{
    private readonly ILogger<CalendarService> _logger;
    private readonly Task<WorkbenchSettings> _settings;
    private readonly TimeProvider _timeProvider;

    public CalendarService(Task<WorkbenchSettings> workbenchSettings, TimeProvider timeProvider,
        ILogger<CalendarService> logger)
    {
        _settings = workbenchSettings ?? throw new ArgumentNullException(nameof(workbenchSettings));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _logger.LogInformation("CalendarService initialized.");
    }

    public IEnumerable<int> GetDays(int year)
    {
        // Input validation
        if (year < 1)
        {
            _logger.LogWarning("Invalid year {Year} requested for GetDays", year);
            return [];
        }

        _logger.LogDebug("Getting days for year {Year}", year);

        // Use Enumerable.Range for better performance and readability
        return Enumerable.Range(1, 25);
    }

    public async Task<bool> YearAvailable(int year)
    {
        // Input validation
        if (year < 1)
        {
            _logger.LogWarning("Invalid year {Year} for YearAvailable check", year);
            return false;
        }

        var settings = await _settings;
        var currentYear = _timeProvider.GetUtcNow().Year;
        var isAvailable = year >= settings.FirstPuzzleYear && year <= currentYear;

        _logger.LogDebug(
            "Year {Year} availability: {IsAvailable}. Valid range: {FirstPuzzleYear}-{CurrentYear}",
            year, isAvailable, settings.FirstPuzzleYear, currentYear);

        return isAvailable;
    }

    public async Task<bool> IsUnlocked(int year, int day)
    {
        // Input validation
        if (year < 1 || day < 1 || day > 25)
        {
            _logger.LogWarning("Invalid year {Year} or day {Day} for IsUnlocked check", year, day);
            return false;
        }

        var currentUtc = _timeProvider.GetUtcNow();

        // More efficient date construction using DateTimeOffset constructor with DateTimeKind
        var settings = await _settings;
        var openUtc = new DateTimeOffset(
            year,
            12,
            day,
            settings.DailyPuzzleOpenTime.Hour,
            settings.DailyPuzzleOpenTime.Minute,
            0,
            TimeSpan.Zero
        );

        var isUnlocked = currentUtc >= openUtc;

        _logger.LogDebug(
            "Day {Day} of year {Year} unlock status: {IsUnlocked}. Current UTC: {CurrentUtcTime}, Opens at: {OpenUtcTime}",
            day, year, isUnlocked, currentUtc, openUtc);

        return isUnlocked;
    }
}
