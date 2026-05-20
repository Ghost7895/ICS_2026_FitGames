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
    private readonly IUserSessionService _sessionService;
    private readonly IUserFacade _userFacade;
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
        IUserFacade userFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        IUserSessionService sessionService) 
        : base(messengerService)
    {
        _gameFacade = gameFacade;
        _userFacade = userFacade;
        _sessionService = sessionService;
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
        var games = await _gameFacade.FilterGamesAsync(SearchText, SelectedGenre, SelectedPegi, SortAscending);

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
        SortAscending = true;
        await LoadGamesAsync();
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        var currentUser = _sessionService.CurrentUser;
        if (currentUser is null) throw new ArgumentNullException(nameof(currentUser), "currentUser cannot be null");

        var userDetail = await _userFacade.GetAsync(currentUser.Id);
        if (userDetail is null) throw new ArgumentNullException(nameof(userDetail), "userDetail cannot be null");

        await _navigationService.GoToAsync(
            NavigationService.GameDetailRouteRelative,
            new Dictionary<string, object?>
            {
                [nameof(GameDetailViewModel.Id)] = id,
                ["LibraryId"] = userDetail.LibraryId,
                ["IsHome"] = true
            });
    }
}
