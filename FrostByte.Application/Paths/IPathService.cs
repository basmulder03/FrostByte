namespace FrostByte.Infrastructure.Paths;

public interface IPathService
{
    /// <summary>
    /// Root folder for the application data.
    /// </summary>
    public string AppDataRoot { get; }

    /// <summary>
    /// Folder where puzzle data is cached.
    /// </summary>
    public string PuzzleCacheRoot { get; }

    /// <summary>
    /// Folder under puzzle root cache for a specific year.
    /// </summary>
    public string PuzzleYearFolder(int year);
}
