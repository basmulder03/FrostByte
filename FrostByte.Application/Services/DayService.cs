using System.Text.Json;
using FrostByte.Application.Clients;
using FrostByte.Application.Models;
using FrostByte.Application.Parsing;
using FrostByte.Infrastructure.Paths;
using Microsoft.Extensions.Logging;

namespace FrostByte.Application.Services;

public class DayService(
    IPathService pathService,
    IAdventOfCodeHttpClient httpClient,
    IPuzzleTransformer transformer,
    ILogger<DayService> logger)
    : IDayService
{
    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private readonly IAdventOfCodeHttpClient _adventOfCodeClient =
        httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    private readonly ILogger<DayService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IPathService _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));

    private readonly IPuzzleTransformer _puzzleTransformer =
        transformer ?? throw new ArgumentNullException(nameof(transformer));

    public async Task<PuzzleDto> GetPuzzleAsync(int year, int day, bool forceRefresh = false)
    {
        _logger.LogInformation("Fetching puzzle for year {Year}, day {Day}", year, day);
        var folder = _pathService.GetPuzzleYearFolder(year);
        var jsonFile = Path.Combine(folder, $"day{day}.json");
        var htmlFile = Path.Combine(folder, $"day{day}.html");

        if (forceRefresh)
        {
            if (File.Exists(jsonFile))
            {
                _logger.LogInformation("Deleting existing puzzle JSON file at {JsonPath}", jsonFile);
                File.Delete(jsonFile);
            }

            if (File.Exists(htmlFile))
            {
                _logger.LogInformation("Deleting existing puzzle HTML file at {HtmlPath}", htmlFile);
                File.Delete(htmlFile);
            }

            _logger.LogInformation("Puzzle files deleted, fetching new puzzle from server...");
        }

        if (File.Exists(jsonFile))
        {
            _logger.LogInformation("Puzzle file found at {JsonPath}", jsonFile);
            return await ParseJsonAsync(jsonFile);
        }

        if (File.Exists(htmlFile))
        {
            _logger.LogInformation("Puzzle HTML file found at {HtmlPath}, parsing to json...", htmlFile);
            return await ParseHtmlToJsonAsync(year, day, await File.ReadAllTextAsync(htmlFile));
        }

        _logger.LogWarning("Puzzle files not found for year {Year}, day {Day}. Fetching from server...", year, day);
        var resp = await _adventOfCodeClient.GetPuzzleHtmlAsync(year, day);
        Directory.CreateDirectory(folder);
        await File.WriteAllTextAsync(htmlFile, resp);
        _logger.LogInformation("Puzzle HTML fetched and saved to {HtmlPath}", htmlFile);
        return await ParseHtmlToJsonAsync(year, day, resp);
    }

    public async Task<string> GetPuzzleInputAsync(int year, int day, bool forceRefresh = false)
    {
        _logger.LogInformation("Fetching puzzle input for year {Year}, day {Day}", year, day);
        var folder = _pathService.GetPuzzleYearFolder(year);
        var inputFile = Path.Combine(folder, $"day{day}_input.txt");
        if (forceRefresh)
        {
            if (File.Exists(inputFile))
            {
                _logger.LogInformation("Deleting existing puzzle input file at {InputPath}", inputFile);
                File.Delete(inputFile);
            }

            _logger.LogInformation("Puzzle input file deleted, fetching new input from server...");
        }

        if (File.Exists(inputFile))
        {
            _logger.LogInformation("Puzzle input file found at {InputPath}", inputFile);
            return await File.ReadAllTextAsync(inputFile);
        }

        _logger.LogWarning("Puzzle input file not found for year {Year}, day {Day}. Fetching from server...", year,
            day);
        var input = await _adventOfCodeClient.GetPuzzleInputAsync(year, day);
        Directory.CreateDirectory(folder);
        await File.WriteAllTextAsync(inputFile, input);
        _logger.LogInformation("Puzzle input fetched and saved to {InputPath}", inputFile);
        return input;
    }

    private async Task<PuzzleDto> ParseHtmlToJsonAsync(int year, int day, string html)
    {
        var dto = _puzzleTransformer.Transform(html, year, day);
        var jsonFile = Path.Combine(_pathService.GetPuzzleYearFolder(year), $"day{day}.json");
        await WriteJsonFileAsync(jsonFile, dto);
        return dto;
    }

    private static async Task WriteJsonFileAsync(string jsonFile, PuzzleDto dto)
    {
        var serialized = JsonSerializer.Serialize(dto, _jsonOpts);
        await File.WriteAllTextAsync(jsonFile, serialized);
    }

    private static async Task<PuzzleDto> ParseJsonAsync(string jsonFile)
    {
        var json = await File.ReadAllTextAsync(jsonFile);
        return JsonSerializer.Deserialize<PuzzleDto>(json, _jsonOpts)
               ?? throw new InvalidOperationException($"Failed to deserialize puzzle from {jsonFile}");
    }
}
