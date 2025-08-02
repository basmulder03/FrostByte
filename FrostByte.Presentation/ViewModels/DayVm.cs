using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrostByte.Application.Models;
using FrostByte.Application.Services;
using Microsoft.Extensions.Logging;

namespace FrostByte.Presentation.ViewModels;

public partial class DayVm(IDayService dayService, ILogger<DayVm> logger) : ObservableObject
{
    private readonly ILogger<DayVm> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private int _day;

    private PuzzleDto? _puzzle;

    private int _year;

    public int Year
    {
        get => _year;
        set => SetProperty(ref _year, value);
    }

    public int Day
    {
        get => _day;
        set => SetProperty(ref _day, value);
    }

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
