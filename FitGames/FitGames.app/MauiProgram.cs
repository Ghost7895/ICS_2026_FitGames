using FitGames.BL;
using FitGames.DAL;
using FitGames.DAL.Options;
using FitGames.DAL.Seeds;
using FitGames.app.ViewModels.Library;
using FitGames.app.Views.Library;
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
            builder.Services.AddTransient<LibraryViewModel>();

            // Register Views 
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<LibraryPage>();

            builder.Services.AddSingleton<IDbSeeder, DbSeeder>();

            return builder.Build();
        }
    }
}
