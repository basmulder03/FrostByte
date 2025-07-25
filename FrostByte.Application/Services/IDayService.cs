namespace FrostByte.Application.Services;

public interface IDayService
{
    Task GetPuzzleAsync(int year, int day);
}
