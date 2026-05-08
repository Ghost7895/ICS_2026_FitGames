using FitGames.app.Models;
using FitGames.app.Services.Interfaces;
using FitGames.app.Views.Library;
using FitGames.app.Views.SignIn;
using FitGames.app.ViewModels.Library;
using FitGames.app.ViewModels.SignIn;

namespace FitGames.app.Services;

public class NavigationService : INavigationService
{
    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new("//signin", typeof(SignInPage), typeof(SignInViewModel)),
        new("//library", typeof(LibraryPage), typeof(LibraryViewModel))
    };

    public async Task GoToAsync(string route)
        => await Shell.Current.GoToAsync(route);

    public async Task GoToAsync(string route, IDictionary<string, object?> parameters)
        => await Shell.Current.GoToAsync(route, parameters);

    public bool SendBackButtonPressed()
        => Shell.Current.SendBackButtonPressed();
}
