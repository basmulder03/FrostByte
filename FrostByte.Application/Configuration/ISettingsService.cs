namespace FrostByte.Application.Configuration;

public interface ISettingsService
{
    /// <summary>
    /// Gets the current settings (loads defaults if not set).
    /// </summary>
    ValueTask<WorkbenchSettings> LoadAsync(CancellationToken ct = default);

    /// <summary>
    /// Saves back to the disk, creating the file/folder structure if it does not exist.
    /// </summary>
    ValueTask SaveAsync(WorkbenchSettings settings, CancellationToken ct = default);
}
