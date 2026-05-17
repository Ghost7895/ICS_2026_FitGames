using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitGames.app.Services.Interfaces;
using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Models;
using FitGames.DAL.Enums;

namespace FitGames.app.ViewModels.Profile;

public partial class ProfileViewModel : ViewModelBase
{
    private readonly IUserFacade _userFacade;
    private readonly ILibraryFacade _libraryFacade;
    private readonly INavigationService _navigationService;
    private readonly IUserSessionService _userSessionService;

    [ObservableProperty]
    public partial UserDetailModel? CurrentUser { get; set; }

    [ObservableProperty]
    public partial int TotalGames { get; set; }

    [ObservableProperty]
    public partial string FavouriteGenre { get; set; } = "Unknown";

    [ObservableProperty]
    public partial ObservableCollection<GenreCountModel> GenreStatistics { get; set; } = [];

    public ProfileViewModel(
        IUserFacade userFacade,
        ILibraryFacade libraryFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        IUserSessionService userSessionService)
        : base(messengerService)
    {
        _userFacade = userFacade;
        _libraryFacade = libraryFacade;
        _navigationService = navigationService;
        _userSessionService = userSessionService;
    }

    [RelayCommand]
    private async Task EditProfileAsync()
    {
        if (CurrentUser is null)
        {
            return;
        }

        await _navigationService.GoToAsync("editprofile");
    }

    protected override async Task LoadDataAsync()
    {
        try
        {
            var currentUserId = _userSessionService.CurrentUser?.Id;
            if (currentUserId is null)
            {
                return;
            }

            CurrentUser = await _userFacade.GetAsync(currentUserId.Value);

            if (CurrentUser is null)
            {
                return;
            }

            // Get user's library to count games
            var library = await _libraryFacade.GetAsync(CurrentUser.LibraryId);

            if (library is null)
            {
                TotalGames = 0;
                return;
            }

            TotalGames = library.Games.Count;

            // Calculate genre statistics
            var genreGroups = library.Games
                .GroupBy(g => g.Genre)
                .OrderByDescending(g => g.Count())
                .Select(g => new GenreCountModel
                {
                    Genre = g.Key,
                    Count = g.Count()
                })
                .ToList();

            GenreStatistics = new ObservableCollection<GenreCountModel>(genreGroups);

            // Set favourite genre
            FavouriteGenre = genreGroups.FirstOrDefault()?.Genre.ToString() ?? "Unknown";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading profile data: {ex.Message}");
        }
    }
}

/// <summary>
/// Helper model for displaying genre statistics
/// </summary>
public class GenreCountModel
{
    public Genre Genre { get; set; }
    public int Count { get; set; }
}
