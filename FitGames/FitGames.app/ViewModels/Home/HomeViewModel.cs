using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitGames.app.Services;
using FitGames.app.Services.Interfaces;
using FitGames.app.ViewModels.Game;
using FitGames.BL.Facades;
using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Models;
using FitGames.DAL.Enums;
using System.Collections.ObjectModel;

namespace FitGames.app.ViewModels.Home;

public partial class HomeViewModel : ViewModelBase
{
    private readonly IGameFacade _gameFacade;
    private readonly ILibraryFacade _libraryFacade;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    public partial ObservableCollection<GameListModel> Games { get; set; } = new();

    [ObservableProperty]
    public partial string SearchText { get; set; } = string.Empty;

    partial void OnSearchTextChanged(string value)
        => _ = LoadGamesAsync();

    [ObservableProperty]
    public partial bool IsFilterVisible { get; set; }

    [ObservableProperty]
    public partial Genre? SelectedGenre { get; set; }

    partial void OnSelectedGenreChanged(Genre? value)
        => _ = LoadGamesAsync();

    [ObservableProperty]
    public partial Pegi? SelectedPegi { get; set; }

    partial void OnSelectedPegiChanged(Pegi? value)
        => _ = LoadGamesAsync();

    [ObservableProperty]
    public partial bool SortAscending { get; set; } = true;

    public ObservableCollection<Genre> AvailableGenres { get; } = new(Enum.GetValues<Genre>().Where(g => g != Genre.Unknown));
    public ObservableCollection<Pegi> AvailablePegis { get; } = new(Enum.GetValues<Pegi>().Where(p => p != Pegi.Unknown));

    public HomeViewModel(
        IGameFacade gameFacade,
        ILibraryFacade libraryFacade,
        INavigationService navigationService,
        IMessengerService messengerService) 
        : base(messengerService)
    {
        _gameFacade = gameFacade;
        _libraryFacade = libraryFacade;
        _navigationService = navigationService;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        await LoadGamesAsync();
    }

    [RelayCommand]
    private async Task LoadGamesAsync()
    {
        IEnumerable<GameListModel> games;

        if (!string.IsNullOrWhiteSpace(SearchText) || (SelectedGenre.HasValue && SelectedGenre.Value != Genre.Unknown) || (SelectedPegi.HasValue && SelectedPegi.Value != Pegi.Unknown))
        {
            games = await _gameFacade.FilterGamesAsync(SearchText, SelectedGenre, SelectedPegi, SortAscending);
        }
        else
        {
            games = await _gameFacade.GetPagingAsync(1, 100);
            
            games = SortAscending
                ? games.OrderBy(g => g.Name).ToList()
                : games.OrderByDescending(g => g.Name).ToList();
        }

        Games.Clear();
        foreach (var game in games)
        {
            Games.Add(game);
        }
    }

    [RelayCommand]
    private void ToggleFilter()
    {
        IsFilterVisible = !IsFilterVisible;
    }

    [RelayCommand]
    private void ToggleSort()
    {
        SortAscending = !SortAscending;
        _ = LoadGamesAsync();
    }

    [RelayCommand]
    private async Task ClearFilterAsync()
    {
        SearchText = string.Empty;
        SelectedGenre = null;
        SelectedPegi = null;
        IsFilterVisible = false;
        SortAscending = true;
        await LoadGamesAsync();
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        IEnumerable<LibraryListModel> libraries = await _libraryFacade.GetPagingAsync(1, 1);
        LibraryListModel? first = libraries.FirstOrDefault();
        if (first is null)
        {
            throw new ArgumentNullException(nameof(first),"Library cannot be null");
        }
        await _navigationService.GoToAsync(
            NavigationService.GameDetailRouteRelative,
            new Dictionary<string, object?>
            {
                [nameof(GameDetailViewModel.Id)] = id,
                ["LibraryId"] = first.Id,
                ["IsHome"] = true
            });
    }
}
