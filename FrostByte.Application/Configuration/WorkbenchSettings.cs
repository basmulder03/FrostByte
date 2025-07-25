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

    /// <summary>
    /// The unique name of the application, used for folder names and settings.
    /// </summary>
    public string ApplicationName { get; set; } = "FrostByte";

    /// <summary>
    /// The folder where application data is stored.
    /// </summary>
    public string AppDataFolder { get; set; } = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(ApplicationName));

    public string CacheFolderName { get; set; } = "Cache";
}
