using FrostByte.Domain.ValueObjects;

namespace FrostByte.Domain.Policies;

public static class AdventRules
{
    public static DateTimeOffset UnlockInstantUtc(Year Year, DayOfAdvent day, TimeSpan openTimeUtc)
    {
        return new DateTimeOffset(Year.Value, 12, day.Value, openTimeUtc.Hours, openTimeUtc.Minutes, 0, TimeSpan.Zero);
    }

    public static bool IsUnlocked(DateTimeOffset nowUtc, Year year, DayOfAdvent day, TimeSpan openTimeUtc)
    {
        return nowUtc >= UnlockInstantUtc(year, day, openTimeUtc);
    }
}
