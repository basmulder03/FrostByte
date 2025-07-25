namespace FrostByte.Application.Models;

public sealed record PuzzleDto(
    int Year,
    int Day,
    string Title,
    IReadOnlyList<PartDto> Parts
);
