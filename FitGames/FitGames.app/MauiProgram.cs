using FitGames.app.ViewModels.Library;
using FitGames.app.ViewModels.SignIn;
using FitGames.app.Views.Library;
using FitGames.app.Views.SignIn;
using FitGames.app.Services;
using FitGames.app.Services.Interfaces;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm.Messaging;

namespace FitGames.app
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Register Services
            builder.Services.AddSingleton<IMessenger>(_ => WeakReferenceMessenger.Default);
            builder.Services.AddSingleton<IMessengerService, MessengerService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IAlertService, AlertService>();

            // Register ViewModels
            builder.Services.AddTransient<SignInViewModel>();
            builder.Services.AddTransient<LibraryViewModel>();

            // Register Views
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<SignInPage>();
            builder.Services.AddTransient<LibraryPage>();

            return builder.Build();
        }
    }
}
