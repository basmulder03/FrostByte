namespace FrostByte.Domain.ValueObjects;

/// <summary>
///     Number of stars earned for a day (0..2). Immutable value object with clamped addition.
/// </summary>
public readonly record struct StarCount
{
    private StarCount(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static StarCount None => new(0);
    public static StarCount One => new(1);
    public static StarCount Two => new(2);

    public bool HasFirstStar => Value >= 1;
    public bool HasSecondStar => Value == 2;

    public static StarCount From(int value)
    {
        if (value is < 0 or > 2)
            throw new ArgumentOutOfRangeException(nameof(value), "Star count must be between 0 and 2.");
        return new StarCount(value);
    }

    public StarCount Add(int stars)
    {
        return From(Math.Clamp(Value + stars, 0, 2));
    }

    public static StarCount operator +(StarCount a, StarCount b)
    {
        return a.Add(b.Value);
    }

    public static StarCount operator +(StarCount a, int b)
    {
        return a.Add(b);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
