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
    IMessengerService messengerService)
    : ViewModelBase(messengerService),
      IRecipient<LibraryGameAddMessage>,
      IRecipient<LibraryGameRemoveMessage>
{
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
    private bool _isPegiDropdownOpen = false;

    [ObservableProperty]
    private bool _isGenreDropdownOpen = false;

    public string PegiButtonLabel => SelectedPegi switch
    {
        Pegi.Pegi3 => "PEGI 3+",
        Pegi.Pegi7 => "PEGI 7+",
        Pegi.Pegi12 => "PEGI 12+",
        Pegi.Pegi16 => "PEGI 16+",
        Pegi.Pegi18 => "PEGI 18+",
        _ => "PEGI"
    };

    public string GenreButtonLabel => SelectedGenre switch
    {
        Genre.Action => "Action",
        Genre.RolePlaying => "RPG",
        Genre.Adventure => "Adventure",
        Genre.Sandbox => "Sandbox",
        Genre.Puzzle => "Puzzle",
        _ => "Genre"
    };



    partial void OnSearchTextChanged(string value)
        => _ = LoadDataAsync();

    partial void OnSortAscendingChanged(bool value)
        => _ = LoadDataAsync();

    partial void OnSelectedPegiChanged(Pegi? value)
    {
        IsPegiDropdownOpen = false;
        OnPropertyChanged(nameof(PegiButtonLabel));
        _ = LoadDataAsync();
    }

    partial void OnSelectedGenreChanged(Genre? value)
    {
        IsGenreDropdownOpen = false;
        OnPropertyChanged(nameof(GenreButtonLabel));
        _ = LoadDataAsync();
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        await LoadTitleAsync();

        Games = await LoadGamesAsync();
    }

    private async Task LoadTitleAsync()
    {
        IEnumerable<UserListModel> users = await userFacade.GetPagingAsync(1, 1);
        UserListModel? first = users.FirstOrDefault();
        LibraryTitle = first is not null
            ? $"{first.Username}'s Library"
            : "Your Library";
    }

    private async Task<IEnumerable<GameListModel>> LoadGamesAsync()
    {
        IEnumerable<LibraryListModel> libraries = await libraryFacade.GetPagingAsync(1, 50);
        LibraryListModel? first = libraries.FirstOrDefault();
        if (first is null) return [];

        LibraryDetailModel? detail = await libraryFacade.GetAsync(first.Id);
        if (detail is null) return [];

        HashSet<Guid> libraryGameIds = detail.Games.Select(g => g.Id).ToHashSet();

        // Base query — name search or all sorted
        IEnumerable<GameListModel> result = string.IsNullOrWhiteSpace(SearchText)
            ? await gameFacade.GetGamesSortedByNameAsync(SortAscending)
            : await gameFacade.FilterGamesByNameAsync(SearchText);

        // PEGI filter (DB)
        if (SelectedPegi is not null)
        {
            IEnumerable<GameListModel> pegiFiltered = await gameFacade.FilterGamesByPegiAsync(SelectedPegi.Value);
            HashSet<Guid> pegiIds = pegiFiltered.Select(g => g.Id).ToHashSet();
            result = result.Where(g => pegiIds.Contains(g.Id));
        }

        // Genre filter (DB)
        if (SelectedGenre is not null)
        {
            IEnumerable<GameListModel> genreFiltered = await gameFacade.FilterGamesByGenreAsync(SelectedGenre.Value);
            HashSet<Guid> genreIds = genreFiltered.Select(g => g.Id).ToHashSet();
            result = result.Where(g => genreIds.Contains(g.Id));
        }

        // If searching, sort was not applied by DB so apply in memory
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            result = SortAscending
                ? result.OrderBy(g => g.Name)
                : result.OrderByDescending(g => g.Name);
        }

        return result.Where(g => libraryGameIds.Contains(g.Id));
    }

    [RelayCommand]
    private void ToggleSort()
        => SortAscending = !SortAscending;

    [RelayCommand]
    private void TogglePegiDropdown()
    {
        IsPegiDropdownOpen = !IsPegiDropdownOpen;
        IsGenreDropdownOpen = false;
    }

    [RelayCommand]
    private void ToggleGenreDropdown()
    {
        IsGenreDropdownOpen = !IsGenreDropdownOpen;
        IsPegiDropdownOpen = false;
    }

    [RelayCommand]
    private void SetPegiFilter(Pegi? pegi)
        => SelectedPegi = SelectedPegi == pegi ? null : pegi;

    [RelayCommand]
    private void SetGenreFilter(Genre? genre)
        => SelectedGenre = SelectedGenre == genre ? null : genre;

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
        => await navigationService.GoToAsync(
            NavigationService.GameDetailRouteRelative,
            new Dictionary<string, object?>
            {
                [nameof(GameDetailViewModel.Id)] = id
            });

    public void Receive(LibraryGameAddMessage message)
        => ForceDataRefreshOnNextAppearing();

    public void Receive(LibraryGameRemoveMessage message)
        => ForceDataRefreshOnNextAppearing();
}