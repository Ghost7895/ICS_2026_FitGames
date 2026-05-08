using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitGames.app.Services.Interfaces;
//using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Models;

namespace FitGames.app.ViewModels.SignIn;

public partial class SignInViewModel : ViewModelBase
{
    //private readonly IUserFacade _userFacade;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    public partial ObservableCollection<UserListModel> Users { get; set; } = [];

    public SignInViewModel(
        IMessengerService messengerService,
        //IUserFacade userFacade,
        INavigationService navigationService)
        : base(messengerService)
    {
        //_userFacade = userFacade;
        _navigationService = navigationService;
    }

    //protected override async Task LoadDataAsync()
    protected override Task LoadDataAsync()
    {
        //var users = await _userFacade.GetPagingAsync(1, 100);
        //Users = new ObservableCollection<UserListModel>(users);
        Users =
        [
            new UserListModel { Id = Guid.NewGuid(), Username = "Samo", Email = "samo@gmail.com" },
            new UserListModel { Id = Guid.NewGuid(), Username = "Adam", Email = "adam@gmail.com" },
            new UserListModel { Id = Guid.NewGuid(), Username = "Filip", Email = "filip@gmail.com" }
        ];
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task SelectUserAsync(UserListModel user)
    {
        await _navigationService.GoToAsync("//library");
    }

    [RelayCommand]
    private async Task AddUserAsync()
    {
        // TODO: navigate to user creation page
        await Task.CompletedTask;
    }
}
