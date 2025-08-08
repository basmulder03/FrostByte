using FrostByte.Domain.Puzzles;
using FrostByte.Domain.ValueObjects;

namespace FrostByte.Domain.Repositories;

public interface IPuzzleRepository
{
    Task<Puzzle?> GetAsync(PuzzleKey key, CancellationToken ct = default);
    Task SaveAsync(Puzzle puzzle, CancellationToken ct = default);
}
