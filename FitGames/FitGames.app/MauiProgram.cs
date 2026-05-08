using FitGames.BL;
using FitGames.DAL;
using FitGames.DAL.Factories;
using FitGames.DAL.Options;
using FitGames.DAL.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FitGames.app.ViewModels.Library;
using FitGames.app.ViewModels.SignIn;
using FitGames.app.Views.Library;
using FitGames.app.ViewModels.CreateUser;
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

            // Configure DAL
            builder.Services.Configure<DALOptions>(options =>
            {
                options.DatabaseDirectory = FileSystem.AppDataDirectory;
                options.DatabaseName = "FitGames.db";
                options.SeedDemoData = true;
            });
            builder.Services.AddSingleton<IDbContextFactory<FitGamesDbContext>>(provider =>
            {
                var dalOptions = provider.GetRequiredService<IOptions<DALOptions>>().Value;
                return new DbContextSqLiteFactory(dalOptions.DatabaseFilePath);
            });

            // Register BL services (facades, mappers, UoW)
            builder.Services.AddBLServices();

            // Register Services
            builder.Services.AddSingleton<IMessenger>(_ => WeakReferenceMessenger.Default);
            builder.Services.AddSingleton<IMessengerService, MessengerService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IAlertService, AlertService>();

            // Register ViewModels
            builder.Services.AddTransient<SignInViewModel>();
            builder.Services.AddTransient<CreateUserViewModel>();
            builder.Services.AddTransient<LibraryViewModel>();

            // Register Views
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<SignInPage>();
            builder.Services.AddTransient<CreateUserPage>();
            builder.Services.AddTransient<LibraryPage>();

            builder.Services.AddSingleton<IDbSeeder, DbSeeder>();

            var app = builder.Build();

            app.Services.GetRequiredService<IDbSeeder>().Seed();

            return app;
        }
    }
}
