using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FitGames.app.Messages;
using FitGames.app.Services.Interfaces;
using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Models;
using FitGames.DAL.Enums;
using System.Collections.ObjectModel;

namespace FitGames.app.ViewModels.Game;

[QueryProperty(nameof(Id), nameof(Id))]
[QueryProperty(nameof(LibraryId), "LibraryId")]
[QueryProperty(nameof(IsHome), "IsHome")]
public partial class GameDetailViewModel(
    IGameFacade gameFacade,
    ILibraryFacade libraryFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService),
      IRecipient<GameEditMessage>,
      IRecipient<GameDeleteMessage>
{
    public Guid Id { get; set; }

    [ObservableProperty]
    public partial GameDetailModel? Game { get; set; }

    [ObservableProperty]
    public partial Guid LibraryId { get; set; }

    [ObservableProperty]
    public partial bool IsHome { get; set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Game = await gameFacade.GetAsync(Id);
    }

    [RelayCommand]
    private void GoBack()
        => navigationService.SendBackButtonPressed();

    [ObservableProperty]
    public partial bool IsUpdateVisible { get; set; }

    [ObservableProperty] public partial string NameInput { get; set; } = string.Empty;
    [ObservableProperty] public partial string DescriptionInput { get; set; } = string.Empty;
    [ObservableProperty] public partial string DeveloperNameInput { get; set; } = string.Empty;
    [ObservableProperty] public partial string ImageUrlInput { get; set; } = string.Empty;
    [ObservableProperty] public partial Genre SelectedGenre { get; set; } = Genre.Unknown;
    [ObservableProperty] public partial Pegi SelectedPegi { get; set; } = Pegi.Unknown;


    public ObservableCollection<Genre> AvailableGenres { get; } = new(Enum.GetValues<Genre>().Where(g => g != Genre.Unknown));
    public ObservableCollection<Pegi> AvailablePegis { get; } = new(Enum.GetValues<Pegi>().Where(p => p != Pegi.Unknown));

    [RelayCommand]
    private void ToggleUpdate()
    {
        IsUpdateVisible = !IsUpdateVisible;

        if (IsUpdateVisible && Game != null)
        {
            SelectedGenre = Game.Genre;
            SelectedPegi = Game.Pegi;
            NameInput = Game.Name;
            DescriptionInput = Game.Description;
            DeveloperNameInput = Game.DeveloperName ?? string.Empty;
            ImageUrlInput = Game.ImageUrl ?? string.Empty;
        }
    }

    [RelayCommand]
    private async Task EditDetailViewGameAsync()
    {
        if (Game == null) return;

        var updatedGame = new GameDetailModel
        {
            Id = Game.Id,
            Name = string.IsNullOrWhiteSpace(NameInput) ? "unspecified" : NameInput.Trim(),
            Description = string.IsNullOrWhiteSpace(DescriptionInput) ? "unspecified" : DescriptionInput.Trim(),
            Genre = SelectedGenre,
            Pegi = SelectedPegi,
            DeveloperName = string.IsNullOrWhiteSpace(DeveloperNameInput) ? "unspecified" : DeveloperNameInput.Trim()
        };

        if (!string.IsNullOrWhiteSpace(ImageUrlInput))
        {
            string tmpUrl = ImageUrlInput.Trim();
            if (Uri.TryCreate(tmpUrl, UriKind.Absolute, out var validatedUri))
            {
                updatedGame.ImageUrl = validatedUri.ToString();
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Error", "Invalid URL detected.", "OK");
                return;
            }
        }

        Game = await gameFacade.SaveAsync(updatedGame);
        WeakReferenceMessenger.Default.Send(new GameEditMessage {GameId = Game.Id});
    }

    [RelayCommand]
    private async Task DeleteGameAsync()
    {
        if (Game == null) return;

        await gameFacade.DeleteAsync(Game.Id);
        WeakReferenceMessenger.Default.Send(new GameDeleteMessage());
        await navigationService.GoToAsync("..");
    }

    [RelayCommand]
    private async Task HandleGameActionAsync()
    {

        if (IsHome)
        {
            await libraryFacade.AddGameToLibraryAsync(LibraryId, Id);
            await Shell.Current.DisplayAlertAsync("Success", "Game has been added to your library.", "OK");
            MessengerService.Send(new LibraryGameAddMessage());
        }
        else
        {
            await libraryFacade.RemoveGameFromLibraryAsync(LibraryId, Id);
            await Shell.Current.DisplayAlertAsync("Success", "Game has been removed from your library.", "OK");
            MessengerService.Send(new LibraryGameRemoveMessage());
        }
    }
    public void Receive(GameEditMessage message)
    {
        if (message.GameId == Game?.Id)
            ForceDataRefreshOnNextAppearing();
    }

    public void Receive(GameDeleteMessage message)
        => ForceDataRefreshOnNextAppearing();
}