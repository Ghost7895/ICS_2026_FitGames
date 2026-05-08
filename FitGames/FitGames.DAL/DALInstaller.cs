using FitGames.DAL.Factories;
using FitGames.DAL.Mappers;
using FitGames.DAL.Options;
using FitGames.DAL.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FitGames.DAL;

public static class DALInstaller
{
    public static IServiceCollection AddDALServices(this IServiceCollection services)
    {
        services.AddSingleton<IDbContextFactory<FitGamesDbContext>>(serviceProvider =>
        {
            DALOptions dalOptions = serviceProvider.GetRequiredService<IOptions<DALOptions>>().Value;
            return new DbContextSqLiteFactory(dalOptions.DatabaseFilePath);
        });

        services.AddSingleton<IDbSeeder, DbSeeder>();

        services.AddSingleton<GameEntityMapper>();
        services.AddSingleton<LibraryEntityMapper>();
        services.AddSingleton<UserEntityMapper>();
        services.AddSingleton<DeveloperEntityMapper>();

        return services;
    }
}