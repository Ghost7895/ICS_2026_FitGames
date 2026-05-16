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
public partial class GameDetailViewModel(
    IGameFacade gameFacade,
    IDeveloperFacade developerFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService),
      IRecipient<GameEditMessage>,
      IRecipient<GameDeleteMessage>
{
    public Guid Id { get; set; }

    [ObservableProperty]
    public partial GameDetailModel? Game { get; set; }

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
    [ObservableProperty] private Genre _selectedGenre = Genre.Unknown;
    [ObservableProperty] private Pegi _selectedPegi = Pegi.Unknown;


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

        Game.Genre = SelectedGenre;
        Game.Pegi = SelectedPegi;
        Game.Name = NameInput?.Trim() ?? string.Empty;
        Game.Description = DescriptionInput?.Trim() ?? string.Empty;
        Game.DeveloperName = DeveloperNameInput?.Trim() ?? "unknown";

        if (!string.IsNullOrWhiteSpace(Game.ImageUrl))
        {
            string tmpUrl = ImageUrlInput.Trim();
            if (Uri.TryCreate(tmpUrl, UriKind.Absolute, out var validatedUri))
            {
                Game.ImageUrl = validatedUri.ToString();
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Error", "Invalid URL detected.", "OK");
                return;
            }
        }

        Game = await gameFacade.SaveAsync(Game);
        WeakReferenceMessenger.Default.Send(new GameEditMessage {GameId = Game.Id});
    }

    public void Receive(GameEditMessage message)
    {
        if (message.GameId == Game?.Id)
            ForceDataRefreshOnNextAppearing();
    }

    public void Receive(GameDeleteMessage message)
        => ForceDataRefreshOnNextAppearing();
}