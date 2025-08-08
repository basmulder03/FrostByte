namespace FrostByte.Domain.ValueObjects;

public readonly record struct PuzzleKey(Year Year, DayOfAdvent Day)
{
    public override string ToString()
    {
        return $"{Year.Value}-{Day.Value:D2}";
    }
}
