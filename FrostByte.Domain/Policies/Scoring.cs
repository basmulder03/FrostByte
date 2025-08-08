namespace FrostByte.Domain.Policies;

public static class Scoring
{
    public static (int newStars, bool firstStar, bool secondStar) EvaluateStars(bool part1Solved, bool part2Solved)
    {
        var stars = (part1Solved ? 1 : 0) + (part2Solved ? 1 : 0);
        return (stars, part1Solved, part2Solved);
    }
}
