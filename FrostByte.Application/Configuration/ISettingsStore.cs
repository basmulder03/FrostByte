using System.Threading;
using System.Threading.Tasks;

namespace FrostByte.Application.Configuration;

/// <summary>
/// Abstraction for persistent storage of application settings.
/// </summary>
public interface ISettingsStore
{
    /// <summary>
    /// Loads settings from persistent storage.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The loaded <see cref="WorkbenchSettings"/> or a new instance if not found.</returns>
    ValueTask<WorkbenchSettings> LoadAsync(CancellationToken ct = default);

    /// <summary>
    /// Saves settings to persistent storage.
    /// </summary>
    /// <param name="settings">The settings to save.</param>
    /// <param name="ct">Cancellation token.</param>
    ValueTask SaveAsync(WorkbenchSettings settings, CancellationToken ct = default);
}
