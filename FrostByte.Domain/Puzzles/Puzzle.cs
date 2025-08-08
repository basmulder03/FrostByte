using FrostByte.Domain.ValueObjects;

namespace FrostByte.Domain.Puzzles;

public sealed record Puzzle(PuzzleKey Key, string Title, IReadOnlyList<PuzzlePart> Parts);
