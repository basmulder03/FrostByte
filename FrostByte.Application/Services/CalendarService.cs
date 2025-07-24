using FrostByte.Application.Configuration;
using Microsoft.Extensions.Options;

namespace FrostByte.Application.Services;

public class CalendarService : ICalendarService
{
    private readonly WorkbenchSettings _settings;
    private readonly TimeProvider _timeProvider;

    public CalendarService(IOptions<WorkbenchSettings> settings, TimeProvider timeProvider)
    {
        _settings = settings.Value;
        _timeProvider = timeProvider;
    }

    public IEnumerable<int> GetDays(int year)
    {
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

        return _timeProvider.GetUtcNow() >= openUtc;
    }
}
