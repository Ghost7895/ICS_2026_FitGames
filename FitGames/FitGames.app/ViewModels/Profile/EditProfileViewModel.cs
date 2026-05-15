using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitGames.app.Services.Interfaces;
using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Models;

namespace FitGames.app.ViewModels.Profile;

public partial class EditProfileViewModel : ViewModelBase
{
    private readonly IUserFacade _userFacade;
    private readonly INavigationService _navigationService;
    private UserDetailModel? _originalUser;

    [ObservableProperty]
    public partial string? Name { get; set; }

    [ObservableProperty]
    public partial string? Surname { get; set; }

    [ObservableProperty]
    public partial string? Email { get; set; }

    [ObservableProperty]
    public partial string? PhoneNumber { get; set; }

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    public EditProfileViewModel(
        IUserFacade userFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _userFacade = userFacade;
        _navigationService = navigationService;
    }

    protected override async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;

            // Get current user
            var allUsers = await _userFacade.GetPagingAsync(1, 1);
            var user = allUsers.FirstOrDefault();

            if (user is null)
            {
                return;
            }

            _originalUser = await _userFacade.GetAsync(user.Id);

            if (_originalUser is null)
            {
                return;
            }

            // Populate form fields
            Name = _originalUser.Name ?? string.Empty;
            Surname = _originalUser.Surname ?? string.Empty;
            Email = _originalUser.Email;
            PhoneNumber = _originalUser.PhoneNumber ?? string.Empty;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading profile data: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task SaveProfileAsync()
    {
        if (_originalUser is null)
        {
            return;
        }

        try
        {
            IsLoading = true;

            // Create updated user model
            var updatedUser = UserDetailModel.Copy(
                _originalUser,
                name: Name,
                surname: Surname,
                email: Email,
                phoneNumber: PhoneNumber);

            // Save to database
            await _userFacade.SaveAsync(updatedUser);

            // Navigate back to profile
            await _navigationService.GoToAsync("..");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving profile: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await _navigationService.GoToAsync("..");
    }
}
