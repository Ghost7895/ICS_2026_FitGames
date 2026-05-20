using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FitGames.app.Messages;
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
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    public partial ObservableCollection<GameListModel> Games { get; set; } = new();

    [ObservableProperty]
    public partial string SearchText { get; set; } = string.Empty;

    partial void OnSearchTextChanged(string value)
        => _ = LoadGamesAsync();

    [ObservableProperty]
    public partial string AddNameInput { get; set; } = string.Empty;
    [ObservableProperty]
    public partial string AddDescriptionInput { get; set; } = string.Empty;
    [ObservableProperty]
    public partial string AddDeveloperNameInput { get; set; } = string.Empty;
    [ObservableProperty]
    public partial string AddImageUrlInput { get; set; } = string.Empty;
    [ObservableProperty]
    public partial Genre AddSelectedGenre { get; set; } = Genre.Unknown;
    [ObservableProperty]
    public partial Pegi AddSelectedPegi { get; set; } = Pegi.Unknown;

    [ObservableProperty]
    public partial bool IsFilterVisible { get; set; }

    [ObservableProperty]
    public partial bool IsAddGameVisible { get; set; }

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
        INavigationService navigationService,
        IMessengerService messengerService,
        IUserSessionService sessionService) 
        : base(messengerService)
    {
        _gameFacade = gameFacade;
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
    private void ToggleAddGame()
    {
        IsAddGameVisible = !IsAddGameVisible;
        AddNameInput = string.Empty;
        AddDescriptionInput = string.Empty;
        AddDeveloperNameInput = string.Empty;
        AddImageUrlInput = string.Empty;
        AddSelectedGenre = Genre.Unknown;
        AddSelectedPegi = Pegi.Unknown;
    }

    [RelayCommand]
    private async Task AddNewGameAsync()
    {

        var newGame = new GameDetailModel
        {
            Id = Guid.Empty,
            Name = AddNameInput?.Trim() ?? string.Empty,
            Description = AddDescriptionInput?.Trim() ?? string.Empty,
            Genre = AddSelectedGenre,
            Pegi = AddSelectedPegi,
            DeveloperName = AddDeveloperNameInput?.Trim() ?? "unknown"
        };

        if (!string.IsNullOrWhiteSpace(AddImageUrlInput))
        {
            string tmpUrl = AddImageUrlInput.Trim();
            if (Uri.TryCreate(tmpUrl, UriKind.Absolute, out var validatedUri))
            {
                newGame.ImageUrl = validatedUri.ToString();
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Error", "Invalid URL detected.", "OK");
                return;
            }
        }

        var game = await _gameFacade.SaveAsync(newGame);
        WeakReferenceMessenger.Default.Send(new GameEditMessage { GameId = game.Id });
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
        if (_sessionService.CurrentUser is null) throw new ArgumentNullException(nameof(_sessionService.CurrentUser), "currentUser cannot be null");

        await _navigationService.GoToAsync(
            NavigationService.GameDetailRouteRelative,
            new Dictionary<string, object?>
            {
                [nameof(GameDetailViewModel.Id)] = id,
                ["LibraryId"] = _sessionService.CurrentUser.LibraryId,
                ["IsHome"] = true
            });
    }
}
