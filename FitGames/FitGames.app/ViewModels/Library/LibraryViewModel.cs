using System.Collections.ObjectModel;
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

namespace FitGames.app.ViewModels.Library;

public partial class LibraryViewModel(
    ILibraryFacade libraryFacade,
    IGameFacade gameFacade,
    IUserFacade userFacade,
    INavigationService navigationService,
    IMessengerService messengerService,
    IUserSessionService sessionService)
    : ViewModelBase(messengerService),
      IRecipient<LibraryGameAddMessage>,
      IRecipient<LibraryGameRemoveMessage>
{
    private Guid? _currentLibraryId;

    [ObservableProperty]
    public partial IEnumerable<GameListModel> Games { get; set; } = [];

    [ObservableProperty]
    public partial string LibraryTitle { get; set; } = "Your Library";

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private bool _sortAscending = true;

    [ObservableProperty]
    private Pegi? _selectedPegi = null;

    [ObservableProperty]
    private Genre? _selectedGenre = null;

    [ObservableProperty]
    public partial bool IsFilterVisible { get; set; }

    public ObservableCollection<Genre> AvailableGenres { get; } = new(Enum.GetValues<Genre>().Where(g => g != Genre.Unknown));
    public ObservableCollection<Pegi> AvailablePegis { get; } = new(Enum.GetValues<Pegi>().Where(p => p != Pegi.Unknown));

    [RelayCommand]
    private async Task LoadGamesManuallyAsync()
    {
        await LoadDataAsync();
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
        _ = LoadDataAsync();
    }

    [RelayCommand]
    private async Task ClearFilterAsync()
    {
        SearchText = string.Empty;
        SelectedGenre = null;
        SelectedPegi = null;
        SortAscending = true;
        await LoadDataAsync();
    }

    partial void OnSearchTextChanged(string value)
        => _ = LoadDataAsync();

    partial void OnSelectedPegiChanged(Pegi? value)
        => _ = LoadDataAsync();

    partial void OnSelectedGenreChanged(Genre? value)
        => _ = LoadDataAsync();

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        await LoadTitleAsync();

        Games = await LoadGamesAsync();
    }

    private async Task LoadTitleAsync()
    {
        var current = sessionService.CurrentUser;
        LibraryTitle = current is not null
            ? $"{current.Username}'s Library"
            : "Your Library";
    }

    private async Task<IEnumerable<GameListModel>> LoadGamesAsync()
    {
        var currentUser = sessionService.CurrentUser;
        if (currentUser is null) return [];

        var userDetail = await userFacade.GetAsync(currentUser.Id);
        if (userDetail is null) return [];

        LibraryDetailModel? detail = await libraryFacade.GetAsync(userDetail.LibraryId);
        if (detail is null) return [];

        _currentLibraryId = userDetail.LibraryId;
        HashSet<Guid> libraryGameIds = detail.Games.Select(g => g.Id).ToHashSet();

        IEnumerable<GameListModel> result = await gameFacade.FilterGamesAsync(SearchText, SelectedGenre, SelectedPegi, SortAscending);

        return result.Where(g => libraryGameIds.Contains(g.Id));
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await navigationService.GoToAsync(
            NavigationService.GameDetailRouteRelative,
            new Dictionary<string, object?>
            {
                [nameof(GameDetailViewModel.Id)] = id,
                ["LibraryId"] = _currentLibraryId,
                ["IsHome"] = false
            });
    }

    public void Receive(LibraryGameAddMessage message)
        => ForceDataRefreshOnNextAppearing();

    public void Receive(LibraryGameRemoveMessage message)
        => ForceDataRefreshOnNextAppearing();
}