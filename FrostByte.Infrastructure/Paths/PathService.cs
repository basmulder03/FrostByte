using FrostByte.Application.Configuration;
using FrostByte.Application.Paths;

namespace FrostByte.Infrastructure.Paths;

public class PathService(Task<WorkbenchSettings> settingsTask) : IPathService
{
    private readonly Task<WorkbenchSettings> _settingsTask =
        settingsTask ?? throw new ArgumentNullException(nameof(settingsTask));

    public async Task<string> GetAppDataRootAsync()
    {
        var settings = await _settingsTask;
        var root = settings.AppDataFolder;
        if (!Directory.Exists(root)) Directory.CreateDirectory(root);
        return root;
    }

    public async Task<string> GetPuzzleCacheRootAsync()
    {
        var settings = await _settingsTask;
        var appDataRoot = settings.AppDataFolder;
        if (!Directory.Exists(appDataRoot)) Directory.CreateDirectory(appDataRoot);
        var cache = Path.Combine(appDataRoot, settings.CacheFolderName);
        if (!Directory.Exists(cache)) Directory.CreateDirectory(cache);
        return cache;
    }

    public async Task<string> GetPuzzleYearFolderAsync(int year)
    {
        var cacheRoot = await GetPuzzleCacheRootAsync();
        var yearFolder = Path.Combine(cacheRoot, year.ToString());
        if (!Directory.Exists(yearFolder)) Directory.CreateDirectory(yearFolder);
        return yearFolder;
    }
}
