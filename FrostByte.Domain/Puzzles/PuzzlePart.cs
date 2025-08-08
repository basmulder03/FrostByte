using FrostByte.Domain.Puzzles.Blocks;

namespace FrostByte.Domain.Puzzles;

public sealed record PuzzlePart(int Number, IReadOnlyList<Block> Blocks);
