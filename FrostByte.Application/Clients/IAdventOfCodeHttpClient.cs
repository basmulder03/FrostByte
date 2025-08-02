namespace FrostByte.Application.Clients;

public interface IAdventOfCodeHttpClient
{
    Task<string> GetPuzzleHtmlAsync(int year, int day);
    Task<string> GetPuzzleInputAsync(int year, int day);
}
