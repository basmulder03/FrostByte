using System.Text.Json;

namespace FrostByte.Application.Configuration;

public class SettingService : ISettingsService
{
    private static readonly JsonSerializerOptions _json = new()
        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private readonly string _filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    // e.g. C:\Users\Me\AppData\Local\FrostByte\settings.json

    public async ValueTask<WorkbenchSettings> LoadAsync(CancellationToken ct = default)
    {
        if (!File.Exists(_filePath))
        {
            return new WorkbenchSettings();
        }

        await using var fs = File.OpenRead(_filePath);
        return await JsonSerializer.DeserializeAsync<WorkbenchSettings>(fs, _json, ct)
               ?? new WorkbenchSettings();
    }

    public async ValueTask SaveAsync(WorkbenchSettings settings, CancellationToken ct = default)
    {
        var dir = Path.GetDirectoryName(_filePath)!;
        Directory.CreateDirectory(dir);

        await using var fs = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(fs, settings, _json, ct);
    }
}
