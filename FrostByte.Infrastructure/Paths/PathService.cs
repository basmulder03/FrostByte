using FrostByte.Application.Configuration;

namespace FrostByte.Infrastructure.Paths;

public class PathService(WorkbenchSettings settings) : IPathService
{
    private readonly WorkbenchSettings _settings = settings ?? throw new ArgumentNullException(nameof(settings));

    public string AppDataRoot
    {
        get
        {
            var root = _settings.AppDataFolder;
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            return root;
        }
    }

    public string PuzzleCacheRoot
    {
        get
        {
            var cache = Path.Combine(AppDataRoot, _settings.CacheFolderName);
            if (!Directory.Exists(cache))
            {
                Directory.CreateDirectory(cache);
            }
            return cache;
        }
    }

    public string GetPuzzleYearFolder(int year)
    {
        var folder = Path.Combine(PuzzleCacheRoot, year.ToString());
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        return folder;
    }
}
