using System.Threading;
using System.Threading.Tasks;

namespace FrostByte.Application.Configuration;

/// <summary>
/// Service for managing application settings, delegating persistence to <see cref="ISettingsStore"/>.
/// </summary>
public class SettingService : ISettingsService
{
    private readonly ISettingsStore _store;

    /// <summary>
    /// Initializes a new instance of <see cref="SettingService"/>.
    /// </summary>
    /// <param name="store">The settings store for persistence.</param>
    public SettingService(ISettingsStore store)
    {
        _store = store;
    }

    /// <summary>
    /// Loads settings from persistent storage via the settings store.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The loaded <see cref="WorkbenchSettings"/> or a new instance if not found.</returns>
    public ValueTask<WorkbenchSettings> LoadAsync(CancellationToken ct = default)
        => _store.LoadAsync(ct);

    /// <summary>
    /// Saves settings to persistent storage via the settings store.
    /// </summary>
    /// <param name="settings">The settings to save.</param>
    /// <param name="ct">Cancellation token.</param>
    public ValueTask SaveAsync(WorkbenchSettings settings, CancellationToken ct = default)
        => _store.SaveAsync(settings, ct);
}
