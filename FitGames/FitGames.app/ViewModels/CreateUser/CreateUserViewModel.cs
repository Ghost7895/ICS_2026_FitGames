using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitGames.app.Services.Interfaces;
using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Models;

namespace FitGames.app.ViewModels.CreateUser;

public partial class CreateUserViewModel : ViewModelBase
{
    private readonly IUserFacade _userFacade;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    public partial string Username { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string? Name { get; set; }

    [ObservableProperty]
    public partial string? Surname { get; set; }

    [ObservableProperty]
    public partial string? PhoneNumber { get; set; }

    public CreateUserViewModel(
        IMessengerService messengerService,
        IUserFacade userFacade,
        INavigationService navigationService)
        : base(messengerService)
    {
        _userFacade = userFacade;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task CreateUserAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email))
            return;

        await _userFacade.SaveAsync(new UserDetailModel
        {
            Id = Guid.Empty,
            Username = Username,
            Email = Email,
            Name = Name,
            Surname = Surname,
            PhoneNumber = PhoneNumber,
            LibraryId = Guid.Empty
        });

        await _navigationService.GoToAsync("..");
    }
}
