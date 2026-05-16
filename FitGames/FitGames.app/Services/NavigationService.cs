using FitGames.app.Models;
using FitGames.app.Services.Interfaces;
using FitGames.app.Views.Game;
using FitGames.app.Views.Library;
using FitGames.app.ViewModels.Game;
using FitGames.app.Views.SignIn;
using FitGames.app.ViewModels.Library;
using FitGames.app.ViewModels.SignIn;

namespace FitGames.app.Services;

public class NavigationService : INavigationService
{
    public const string LibraryRouteAbsolute = "//library";
    public const string GameDetailRouteRelative = "gamedetail";
    public const string LibraryEditRouteRelative = "edit";
    public const string SignInRouteAbsolute = "//signin";

    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new(SignInRouteAbsolute, typeof(SignInPage), typeof(SignInViewModel)),
        new(LibraryRouteAbsolute, typeof(LibraryPage), typeof(LibraryViewModel)),
        new(GameDetailRouteRelative, typeof(GameDetailPage), typeof(GameDetailViewModel)),
    };

    public async Task GoToAsync(string route)
        => await Shell.Current.GoToAsync(route);

    public async Task GoToAsync(string route, IDictionary<string, object?> parameters)
        => await Shell.Current.GoToAsync(route, parameters);

    public bool SendBackButtonPressed()
        => Shell.Current.SendBackButtonPressed();
}