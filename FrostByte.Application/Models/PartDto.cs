namespace FrostByte.Application.Models;

public sealed record PartDto(int PartNumber, IReadOnlyList<Block> Blocks);
