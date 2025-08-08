using System.Text.RegularExpressions;

namespace FrostByte.Domain.ValueObjects;

/// <summary>
///     Stable identifier for a programming language/plugin, e.g. "ts", "csharp", "python".
///     Lowercase, [a-z0-9._-] characters only.
/// </summary>
public readonly partial record struct LanguageId
{
    private static readonly Regex Pattern = MyRegex();

    private LanguageId(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static LanguageId TypeScript => From("ts");
    public static LanguageId CSharp => From("csharp");
    public static LanguageId Python => From("python");

    [GeneratedRegex(@"^[a-z0-9._-]+$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();

    public static LanguageId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Language ID cannot be null or empty.", nameof(value));

        var v = value.Trim().ToLowerInvariant();
        if (!Pattern.IsMatch(v))
            throw new ArgumentException(
                "Language ID must consist of lowercase letters, digits, dots, underscores, or hyphens.", nameof(value));
        return new LanguageId(v);
    }

    public override string ToString()
    {
        return Value;
    }

    public static explicit operator LanguageId(string value)
    {
        return From(value);
    }

    public static implicit operator string(LanguageId id)
    {
        return id.Value;
    }
}
