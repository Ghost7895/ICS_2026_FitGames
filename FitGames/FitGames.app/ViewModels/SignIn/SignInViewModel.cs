using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitGames.app.Services.Interfaces;
using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Models;

namespace FitGames.app.ViewModels.SignIn;

public partial class SignInViewModel : ViewModelBase
{
    private readonly IUserFacade _userFacade;
    private readonly INavigationService _navigationService;
    private readonly IUserSessionService _sessionService;

    [ObservableProperty]
    public partial ObservableCollection<UserListModel> Users { get; set; } = [];

    public SignInViewModel(
        IMessengerService messengerService,
        IUserFacade userFacade,
        INavigationService navigationService,
        IUserSessionService sessionService)
        : base(messengerService)
    {
        _userFacade = userFacade;
        _navigationService = navigationService;
        _sessionService = sessionService;
    }

    protected override async Task LoadDataAsync()
    {
        var users = await _userFacade.GetPagingAsync(1, 100);
        Users = new ObservableCollection<UserListModel>(users);
    }

    [RelayCommand]
    private async Task SelectUserAsync(UserListModel user)
    {
        _sessionService.CurrentUser = user;
        await _navigationService.GoToAsync("//home");
    }

    [RelayCommand]
    private async Task AddUserAsync()
    {
        ForceDataRefreshOnNextAppearing();
        await _navigationService.GoToAsync("createuser");
    }
}
