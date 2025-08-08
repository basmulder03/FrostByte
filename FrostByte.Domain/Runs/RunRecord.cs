using FrostByte.Domain.ValueObjects;

namespace FrostByte.Domain.Runs;

public sealed record RunRecord(
    PuzzleKey PuzzleKey,
    DateTimeOffset StartedUtc,
    TimeSpan CompileDuration,
    TimeSpan ExecutionDuration,
    RunOutcome Outcome,
    string? Message // Diagnostics summary, not raw logs
);
