using FrostByte.Infrastructure.Paths;
using Microsoft.Extensions.Logging;

namespace FrostByte.Application.Services;

public class DayService(IPathService pathService, IHttpClientFactory httpClientFactory, ILogger<DayService> logger)
    : IDayService
{
    private readonly IPathService _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    private readonly ILogger<DayService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Task GetPuzzleAsync(int year, int day)
    {
        _logger.LogInformation("Fetching puzzle for year {Year}, day {Day}", year, day);
        return Task.CompletedTask;
    }
}
