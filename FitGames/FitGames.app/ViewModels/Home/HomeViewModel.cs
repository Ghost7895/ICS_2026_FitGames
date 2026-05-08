using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitGames.app.Services;
using FitGames.app.Services.Interfaces;
using FitGames.app.ViewModels.Game;
using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Models;
using FitGames.DAL.Enums;
using System.Collections.ObjectModel;

namespace FitGames.app.ViewModels.Home;

public partial class HomeViewModel : ViewModelBase
{
    private readonly IGameFacade _gameFacade;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    public partial ObservableCollection<GameListModel> Games { get; set; } = new();

    [ObservableProperty]
    public partial string SearchText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsFilterVisible { get; set; }

    [ObservableProperty]
    public partial Genre? SelectedGenre { get; set; }

    public ObservableCollection<Genre> AvailableGenres { get; } = new(Enum.GetValues<Genre>().Where(g => g != Genre.Unknown));

    public HomeViewModel(
        IGameFacade gameFacade,
        INavigationService navigationService,
        IMessengerService messengerService) 
        : base(messengerService)
    {
        _gameFacade = gameFacade;
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

        if (!string.IsNullOrWhiteSpace(SearchText) || (SelectedGenre.HasValue && SelectedGenre.Value != Genre.Unknown))
        {
            games = await _gameFacade.FilterGamesAsync(SearchText, SelectedGenre);
        }
        else
        {
            // If no search or filter, get all/paging
            games = await _gameFacade.GetPagingAsync(1, 100);
            
            // Randomize sort if no filter applies (as requested)
            games = games.OrderBy(x => Guid.NewGuid()).ToList();
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
    private async Task ClearFilterAsync()
    {
        SearchText = string.Empty;
        SelectedGenre = null;
        IsFilterVisible = false;
        await LoadGamesAsync();
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await _navigationService.GoToAsync(
            NavigationService.GameDetailRouteRelative,
            new Dictionary<string, object?>
            {
                [nameof(GameDetailViewModel.Id)] = id
            });
    }
}
