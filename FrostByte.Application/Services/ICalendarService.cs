namespace FrostByte.Application.Services;

/// <summary>
///     Provides methods to interact with the calendar.
/// </summary>
public interface ICalendarService
{
    /// <summary>
    ///     All days that are available in the calendar for a given year.
    /// </summary>
    /// <param name="year">The year for which to get the days for.</param>
    /// <returns></returns>
    IEnumerable<int> GetDays(int year);

    /// <summary>
    ///     True if puzzle for [year, day] is unlocked, false otherwise.
    /// </summary>
    Task<bool> IsUnlocked(int year, int day);

    /// <summary>
    ///     True if a calendar should be shown for the specified year, false otherwise.
    /// </summary>
    Task<bool> YearAvailable(int year);
}
