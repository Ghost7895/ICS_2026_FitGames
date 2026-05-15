using FitGames.BL;
using FitGames.DAL;
using FitGames.DAL.Options;
using FitGames.DAL.Migrator;
using FitGames.DAL.Seeds;
using FitGames.app.ViewModels.Library;
using FitGames.app.ViewModels.Game;
using FitGames.app.ViewModels.Home;
using FitGames.app.ViewModels.SignIn;
using FitGames.app.ViewModels.CreateUser;
using FitGames.app.Views.Library;
using FitGames.app.Views.Game;
using FitGames.app.Views.Home;
using FitGames.app.Views.CreateUser;
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

            // Configure DAL options
            builder.Services.Configure<DALOptions>(options =>
            {
                options.DatabaseDirectory = FileSystem.AppDataDirectory;
                options.DatabaseName = "FitGames.db";
                options.SeedDemoData = true;
            });

            // Register Services
            builder.Services.AddSingleton<IMessenger>(_ => WeakReferenceMessenger.Default);
            builder.Services.AddSingleton<IMessengerService, MessengerService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IAlertService, AlertService>();
            builder.Services.AddSingleton<IUserSessionService, UserSessionService>();

            // Register DAL + BL
            builder.Services.AddDALServices();
            builder.Services.AddBLServices();

            // Register ViewModels
            builder.Services.AddTransient<SignInViewModel>();
            builder.Services.AddTransient<CreateUserViewModel>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<LibraryViewModel>();
            builder.Services.AddTransient<GameDetailViewModel>();


            // Register Views
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<SignInPage>();
            builder.Services.AddTransient<CreateUserPage>();
            builder.Services.AddTransient<LibraryPage>();
            builder.Services.AddTransient<GameDetailPage>();

            // Register Shell route for GameDetailPage (pushed via GoToAsync)
            Routing.RegisterRoute(NavigationService.GameDetailRouteRelative, typeof(GameDetailPage));

            var app = builder.Build();

            // Migrate and seed
            app.Services.GetRequiredService<IDbMigrator>().Migrate();
            app.Services.GetRequiredService<IDbSeeder>().Seed();
            
            

            return app;
        }
    }
}