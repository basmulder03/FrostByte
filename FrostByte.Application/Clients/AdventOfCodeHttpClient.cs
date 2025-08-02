using Microsoft.Extensions.Logging;

namespace FrostByte.Application.Clients;

public class AdventOfCodeHttpClient(HttpClient httpClient, ILogger<AdventOfCodeHttpClient> logger)
    : IAdventOfCodeHttpClient
{
    private readonly ILogger<AdventOfCodeHttpClient>
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Task<string> GetPuzzleHtmlAsync(int year, int day)
    {
        _logger.LogInformation("Fetching puzzle HTML for year {Year}, day {Day}", year, day);
        return httpClient.GetStringAsync($"/{year}/day/{day}");
    }

    public Task<string> GetPuzzleInputAsync(int year, int day)
    {
        _logger.LogInformation("Fetching puzzle input for year {Year}, day {Day}", year, day);
        return httpClient.GetStringAsync($"/{year}/day/{day}/input");
    }
}
