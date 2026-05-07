using FitGames.DAL.Options;
using FitGames.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FitGames.DAL.Seeds;

public class DbSeeder(IDbContextFactory<FitGamesDbContext> dbContextFactory, IOptions<DALOptions> options)
    : IDbSeeder
{
    public void Seed()
    {
        using FitGamesDbContext dbContext = dbContextFactory.CreateDbContext();

        if (options.Value.SeedDemoData is false)
        {
            return;
        }

        // ---- Developers ----
        foreach (var developer in new[]
        {
            DeveloperSeeds.Valve,
            DeveloperSeeds.CDProjekt,
            DeveloperSeeds.Rockstar,
            DeveloperSeeds.Naughty,
            DeveloperSeeds.Mojang,
        })
        {
            if (!dbContext.Set<DeveloperEntity>().Any(d => d.Id == developer.Id))
            {
                dbContext.Set<DeveloperEntity>().Add(developer);
            }
        }

        dbContext.SaveChanges();

        // ---- Games ----
        foreach (var game in new[]
        {
            GameSeeds.CS2,
            GameSeeds.Witcher3,
            GameSeeds.GTA5,
            GameSeeds.LastOfUs,
            GameSeeds.Minecraft,
            GameSeeds.Portal2,
        })
        {
            if (!dbContext.Set<GameEntity>().Any(g => g.Id == game.Id))
            {
                dbContext.Set<GameEntity>().Add(
                    game with { Developer = null! });
            }
        }

        dbContext.SaveChanges();

        // ---- Libraries ----
        foreach (var library in new[]
        {
            LibrarySeeds.AlicesLibrary,
            LibrarySeeds.BobsLibrary,
            LibrarySeeds.SharedLibrary,
        })
        {
            if (!dbContext.Set<LibraryEntity>().Any(l => l.Id == library.Id))
            {
                dbContext.Set<LibraryEntity>().Add(library);
            }
        }

        dbContext.SaveChanges();

        // ---- Users ----
        foreach (var user in new[]
        {
            UserSeeds.Alice,
            UserSeeds.Bob,
        })
        {
            if (!dbContext.Set<UserEntity>().Any(u => u.Id == user.Id))
            {
                dbContext.Set<UserEntity>().Add(
                    user with { Library = null! });
            }
        }

        dbContext.SaveChanges();

        // ---- LibraryGames (join table) ----
        foreach (var libraryGame in new[]
        {
            LibraryGameSeeds.AliceCS2,
            LibraryGameSeeds.AliceWitcher3,
            LibraryGameSeeds.AliceMinecraft,
            LibraryGameSeeds.BobGTA5,
            LibraryGameSeeds.BobLastOfUs,
            LibraryGameSeeds.BobPortal2,
            LibraryGameSeeds.SharedWitcher3,
            LibraryGameSeeds.SharedMinecraft,
            LibraryGameSeeds.SharedPortal2,
        })
        {
            if (!dbContext.Set<LibraryGameEntity>().Any(
                    lg => lg.LibraryId == libraryGame.LibraryId && lg.GameId == libraryGame.GameId))
            {
                dbContext.Set<LibraryGameEntity>().Add(
                    libraryGame with { Library = null!, Game = null! });
            }
        }

        dbContext.SaveChanges();
    }
}