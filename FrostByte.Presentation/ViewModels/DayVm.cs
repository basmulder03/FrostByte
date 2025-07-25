using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrostByte.Application.Models;
using FrostByte.Application.Services;
using Microsoft.Extensions.Logging;

namespace FrostByte.Presentation.ViewModels;

[QueryProperty(nameof(Year), "year")]
[QueryProperty(nameof(Day), "day")]
public partial class DayVm(IDayService dayService, ILogger<DayVm> logger) : ObservableObject
{
    private readonly ILogger<DayVm> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private int _year;
    public int Year
    {
        get => _year;
        set => SetProperty(ref _year, value);
    }

    private int _day;
    public int Day
    {
        get => _day;
        set => SetProperty(ref _day, value);
    }

    private PuzzleDto? _puzzle;
    public PuzzleDto? Puzzle
    {
        get => _puzzle;
        set => SetProperty(ref _puzzle, value);
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        _logger.LogInformation("Loading puzzle for year {Year}, day {Day}", Year, Day);
        Puzzle = await dayService.GetPuzzleAsync(Year, Day);
    }
}
