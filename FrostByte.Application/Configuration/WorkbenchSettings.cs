using Microsoft.VisualBasic;

namespace FrostByte.Application.Configuration;

public class WorkbenchSettings
{
    /// <summary>
    /// Time of day (UTC) when the daily puzzle opens.
    /// </summary>
    public TimeOnly DailyPuzzleOpenTime { get; set; } = new(5, 0);

    /// <summary>
    /// The first year that has puzzles available.
    /// </summary>
    public int FirstPuzzleYear { get; set; } = 2015;
}
