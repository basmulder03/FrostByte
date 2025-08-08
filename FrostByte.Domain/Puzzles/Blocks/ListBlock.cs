namespace FrostByte.Domain.Puzzles.Blocks;

public sealed record ListBlock(IReadOnlyList<string> Items) : Block;
