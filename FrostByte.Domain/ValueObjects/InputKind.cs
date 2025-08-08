namespace FrostByte.Domain.ValueObjects;

/// <summary>
///     Which input is being used to run the solution: the real AoC input or a sample/test input.
///     String-backed to allow future variants (e.g., "sample-2") without changing an enum.
/// </summary>
public readonly record struct InputKind
{
    public static readonly InputKind Real = new("real");
    public static readonly InputKind Sample = new("sample");

    private InputKind(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public bool IsReal => this == Real;
    public bool IsSample => this == Sample;

    public static InputKind From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Input kind cannot be null or empty.", nameof(value));

        var v = value.Trim().ToLowerInvariant();
        return v switch
        {
            "real" => Real,
            "sample" => Sample,
            _ => new InputKind(v)
        };
    }

    public override string ToString()
    {
        return Value;
    }

    public static explicit operator InputKind(string value)
    {
        return From(value);
    }

    public static implicit operator string(InputKind inputKind)
    {
        return inputKind.Value;
    }
}
