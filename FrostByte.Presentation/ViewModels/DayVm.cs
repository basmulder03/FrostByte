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

    private bool _isLoading;

    private PuzzleDto? _puzzle;

    private string? _puzzleInput;

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

    public string? PuzzleInput
    {
        get => _puzzleInput;
        set => SetProperty(ref _puzzleInput, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsLoading = true;
        try
        {
            _logger.LogInformation("Loading puzzle for year {Year}, day {Day}", Year, Day);
            Puzzle = await dayService.GetPuzzleAsync(Year, Day);
            PuzzleInput = await dayService.GetPuzzleInputAsync(Year, Day);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
