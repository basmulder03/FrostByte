using FrostByte.Application.Configuration;
using CommunityToolkit.Maui.Storage;

namespace FrostByte.Infrastructure.Services;

public class WindowsSecretStore : ISecretStore
{
    private const string Prefix = "FrostByte";
    private const string Suffix = "Secret";
    private const string Divider = "|";

    public Task SetAsync(string key, string value, DateTimeOffset? expiresUtc = null)
    {
        return SecureStorage.Default.SetAsync(Key(key), Pack(value, expiresUtc));
    }

    public async Task<string?> GetAsync(string key)
    {
        var raw = await SecureStorage.Default.GetAsync(Key(key));
        if (raw is null) return null;
        var (unpackedValue, expires) = Unpack(raw);
        if (expires >= DateTimeOffset.UtcNow) return unpackedValue;
        await RemoveAsync(key);
        return null;
    }

    public Task RemoveAsync(string key)
    {
        SecureStorage.Default.Remove(Key(key));
        return Task.CompletedTask;
    }

    private static string Key(string key)
    {
        return $"{Prefix}{key}{Suffix}";
    }

    private static string Pack(string v, DateTimeOffset? e)
    {
        return $"{v}{Divider}{e?.ToUnixTimeSeconds() ?? 0}";
    }

    private static (string, DateTimeOffset) Unpack(string v)
    {
        if (!v.Contains(Divider))
            return (v, DateTimeOffset.MaxValue);
        var parts = v.Split(Divider);
        if (parts.Length != 2)
            throw new FormatException("Invalid secret format.");
        var value = parts[0];
        if (!long.TryParse(parts[1], out var unixTime))
            throw new FormatException("Invalid expiration format.");
        var expires = unixTime == 0 ? DateTimeOffset.MaxValue : DateTimeOffset.FromUnixTimeSeconds(unixTime);
        return (value, expires);
    }
}
