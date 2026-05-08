using System.Windows.Input;
using FitGames.app.Services.Interfaces;
using FitGames.app.Views.CreateUser;

namespace FitGames.app
{
    public partial class AppShell : Shell
    {
        private readonly INavigationService _navigationService;
        public ICommand GoToSignInCommand { get; }
        public ICommand GoToHomeCommand { get; }
        public ICommand GoToLibraryCommand { get; }

        public AppShell(INavigationService navigationService)
        {
            _navigationService = navigationService;
            InitializeComponent();

            Routing.RegisterRoute("createuser", typeof(CreateUserPage));

            GoToSignInCommand = new Command(async () => await GoToAsync("//signin"));
            GoToHomeCommand = new Command(async () => await GoToAsync("//home"));
            GoToLibraryCommand = new Command(async () => await GoToAsync("//library"));

            BindingContext = this;
        }
    }
}
