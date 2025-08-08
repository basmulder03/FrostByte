namespace FrostByte.Domain.ValueObjects;

public readonly record struct DayOfAdvent(int Value)
{
    public static DayOfAdvent From(int value)
    {
        if (value is < 1 or > 25)
            throw new ArgumentOutOfRangeException(nameof(value), "Day of Advent must be between 1 and 25.");
        return new DayOfAdvent(value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
