using FrostByte.Application.Models;

namespace FrostByte.Application.Services;

public interface IDayService
{
    Task<PuzzleDto> GetPuzzleAsync(int year, int day, bool forceRefresh = false);

    Task<string> GetPuzzleInputAsync(int year, int day, bool forceRefresh = false);
}
