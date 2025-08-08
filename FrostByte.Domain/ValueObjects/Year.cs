namespace FrostByte.Domain.ValueObjects;

public readonly record struct Year(int Value)
{
    public const int First = 2015;

    public static Year From(int value)
    {
        if (value is < First or > 9999)
            throw new ArgumentOutOfRangeException(nameof(value), $"Year must be between {First} and 9999.");
        return new Year(value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
