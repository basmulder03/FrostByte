using FrostByte.Domain.ValueObjects;

namespace FrostByte.Domain.Runs;

public sealed record RunStats(
    PuzzleKey PuzzleKey,
    int TotalRuns,
    TimeSpan BestExecution,
    DateTimeOffset? FirstCompletedUtc
);
