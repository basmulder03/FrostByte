using System.Text.Json;
using FrostByte.Application.Configuration;
using FrostByte.Application.Paths;

namespace FrostByte.Infrastructure.Services;

/// <summary>
/// Provides persistent storage for application settings using a file in the application data directory.
/// The settings file path is resolved via <see cref="IPathService"/>.
/// </summary>
public class SettingsStore : ISettingsStore
{
    private static readonly JsonSerializerOptions _json = new()
        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private readonly IPathService _pathService;
    private const string SettingsFileName = "settings.json";

    /// <summary>
    /// Initializes a new instance of <see cref="SettingsStore"/>.
    /// </summary>
    /// <param name="pathService">Service for resolving application data paths.</param>
    public SettingsStore(IPathService pathService)
    {
        _pathService = pathService;
    }

    /// <summary>
    /// Gets the full path to the settings file.
    /// </summary>
    /// <returns>Absolute path to the settings file.</returns>
    private async Task<string> GetSettingsFilePathAsync()
    {
        var dir = await _pathService.GetAppDataRootAsync();
        return Path.Combine(dir, SettingsFileName);
    }

    /// <summary>
    /// Loads settings from persistent storage.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The loaded <see cref="WorkbenchSettings"/> or a new instance if not found.</returns>
    public async ValueTask<WorkbenchSettings> LoadAsync(CancellationToken ct = default)
    {
        var filePath = await GetSettingsFilePathAsync();
        if (!File.Exists(filePath))
        {
            return new WorkbenchSettings();
        }

        await using var fs = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<WorkbenchSettings>(fs, _json, ct)
               ?? new WorkbenchSettings();
    }

    /// <summary>
    /// Saves settings to persistent storage.
    /// </summary>
    /// <param name="settings">The settings to save.</param>
    /// <param name="ct">Cancellation token.</param>
    public async ValueTask SaveAsync(WorkbenchSettings settings, CancellationToken ct = default)
    {
        var filePath = await GetSettingsFilePathAsync();
        var dirPath = Path.GetDirectoryName(filePath);
        if (dirPath is not null)
        {
            Directory.CreateDirectory(dirPath);
        }
        await using var fs = File.Create(filePath);
        await JsonSerializer.SerializeAsync(fs, settings, _json, ct);
    }
}
