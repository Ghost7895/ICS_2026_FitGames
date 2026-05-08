using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FitGames.app.Messages;
using FitGames.app.Services;
using FitGames.app.Services.Interfaces;
using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Models;

namespace FitGames.app.ViewModels.Library;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class LibraryDetailViewModel(
    ILibraryFacade libraryFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService),
      IRecipient<LibraryEditMessage>,
      IRecipient<LibraryGameAddMessage>,
      IRecipient<LibraryGameRemoveMessage>
{
    public Guid Id { get; set; }

    [ObservableProperty]
    public partial LibraryDetailModel? Library { get; set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Library = await libraryFacade.GetAsync(Id);
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Library is not null)
        {
            await libraryFacade.DeleteAsync(Library.Id);

            MessengerService.Send(new LibraryDeleteMessage());

            navigationService.SendBackButtonPressed();
        }
    }

    //[RelayCommand]
    //private async Task GoToEditAsync()
    //{
    //    if (Library is not null)
    //    {
    //        await navigationService.GoToAsync(
    //            NavigationService.LibraryEditRouteRelative,
    //            new Dictionary<string, object?>
    //            {
    //                [nameof(LibraryEditViewModel.Id)] = Library.Id
    //            });
    //    }
    //}

    public void Receive(LibraryEditMessage message)
    {
        if (message.LibraryId == Library?.Id)
            ForceDataRefreshOnNextAppearing();
    }

    public void Receive(LibraryGameAddMessage message)
        => ForceDataRefreshOnNextAppearing();

    public void Receive(LibraryGameRemoveMessage message)
        => ForceDataRefreshOnNextAppearing();
}