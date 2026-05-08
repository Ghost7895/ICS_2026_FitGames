using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FitGames.app.Messages;
using FitGames.app.Services.Interfaces;
using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Models;

namespace FitGames.app.ViewModels.Game;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class GameDetailViewModel(
    IGameFacade gameFacade,
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

    public void Receive(GameEditMessage message)
    {
        if (message.GameId == Game?.Id)
            ForceDataRefreshOnNextAppearing();
    }

    public void Receive(GameDeleteMessage message)
        => ForceDataRefreshOnNextAppearing();
}