using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FitGames.app.Messages;
using FitGames.app.Services;
using FitGames.app.Services.Interfaces;
using FitGames.app.ViewModels.Game;
using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Models;

namespace FitGames.app.ViewModels.Library;

public partial class LibraryViewModel(
    ILibraryFacade libraryFacade,
    IGameFacade gameFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService),
      IRecipient<LibraryGameAddMessage>,
      IRecipient<LibraryGameRemoveMessage>
{
    [ObservableProperty]
    public partial IEnumerable<GameListModel> Games { get; set; } = [];

    [ObservableProperty]
    private string _searchText = string.Empty;

    partial void OnSearchTextChanged(string value)
        => _ = LoadDataAsync();

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Games = await LoadGamesAsync();
    }

    private async Task<IEnumerable<GameListModel>> LoadGamesAsync()
    {
        IEnumerable<LibraryListModel> libraries = await libraryFacade.GetPagingAsync(1, 50);
        LibraryListModel? first = libraries.FirstOrDefault();
        if (first is null) return [];

        LibraryDetailModel? detail = await libraryFacade.GetAsync(first.Id);
        if (detail is null) return [];

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            IEnumerable<GameListModel> filtered = await gameFacade.FilterGamesByNameAsync(SearchText);
            HashSet<Guid> libraryGameIds = detail.Games.Select(g => g.Id).ToHashSet();
            return filtered.Where(g => libraryGameIds.Contains(g.Id));
        }

        return detail.Games;
    }

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