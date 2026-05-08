using System.Windows.Input;
using FitGames.app.Services.Interfaces;

namespace FitGames.app
{
    public partial class AppShell : Shell
    {
        private readonly INavigationService _navigationService;
        public ICommand GoToHomeCommand { get; }
        public ICommand GoToLibraryCommand { get; }

        public AppShell(INavigationService navigationService)
        {
            _navigationService = navigationService;
            InitializeComponent();

            GoToHomeCommand = new Command(async () => await GoToAsync("//home"));
            GoToLibraryCommand = new Command(async () => await GoToAsync("//library"));

            BindingContext = this;
        }
    }
}
