namespace FrostByte.Application.Paths;

public interface IPathService
{
    /// <summary>
    ///     Root folder for the application data.
    /// </summary>
    Task<string> GetAppDataRootAsync();

    /// <summary>
    ///     Folder where puzzle data is cached.
    /// </summary>
    Task<string> GetPuzzleCacheRootAsync();

    /// <summary>
    ///     Folder under puzzle root cache for a specific year.
    /// </summary>
    Task<string> GetPuzzleYearFolderAsync(int year);
}
