using FrostByte.Domain.Runs;
using FrostByte.Domain.ValueObjects;

namespace FrostByte.Domain.Repositories;

public interface IRunRepository
{
    Task AddAsync(RunRecord run, CancellationToken ct = default);
    Task<RunStats> GetStatsAsync(PuzzleKey puzzleKey, CancellationToken ct = default);
}
