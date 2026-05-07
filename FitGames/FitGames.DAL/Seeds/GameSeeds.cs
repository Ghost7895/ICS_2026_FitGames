using FitGames.DAL.Entities;
using FitGames.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Seeds;

public static class GameSeeds
{
    public static readonly GameEntity CS2 = new()
    {
        Id = Guid.Parse("20000000-0000-0000-0000-000000000001"),
        Name = "Counter-Strike 2",
        Description = "The world's most popular tactical first-person shooter, rebuilt on the Source 2 engine.",
        Pegi = Pegi.Pegi18,
        Genre = Genre.Action,
        ImageUrl = "https://example.com/images/cs2.jpg",
        DeveloperId = DeveloperSeeds.Valve.Id,
        Developer = DeveloperSeeds.Valve
    };

    public static readonly GameEntity Witcher3 = new()
    {
        Id = Guid.Parse("20000000-0000-0000-0000-000000000002"),
        Name = "The Witcher 3: Wild Hunt",
        Description = "An open-world RPG following Geralt of Rivia on a quest to find his adopted daughter.",
        Pegi = Pegi.Pegi18,
        Genre = Genre.RolePlaying,
        ImageUrl = "https://example.com/images/witcher3.jpg",
        DeveloperId = DeveloperSeeds.CDProjekt.Id,
        Developer = DeveloperSeeds.CDProjekt
    };

    public static readonly GameEntity GTA5 = new()
    {
        Id = Guid.Parse("20000000-0000-0000-0000-000000000003"),
        Name = "Grand Theft Auto V",
        Description = "An open-world crime game set in the fictional state of San Andreas.",
        Pegi = Pegi.Pegi18,
        Genre = Genre.Action,
        ImageUrl = "https://example.com/images/gta5.jpg",
        DeveloperId = DeveloperSeeds.Rockstar.Id,
        Developer = DeveloperSeeds.Rockstar
    };

    public static readonly GameEntity LastOfUs = new()
    {
        Id = Guid.Parse("20000000-0000-0000-0000-000000000004"),
        Name = "The Last of Us Part I",
        Description = "A survival action-adventure game set in a post-apocalyptic world overrun by infected humans.",
        Pegi = Pegi.Pegi18,
        Genre = Genre.Adventure,
        ImageUrl = "https://example.com/images/tlou.jpg",
        DeveloperId = DeveloperSeeds.Naughty.Id,
        Developer = DeveloperSeeds.Naughty
    };

    public static readonly GameEntity Minecraft = new()
    {
        Id = Guid.Parse("20000000-0000-0000-0000-000000000005"),
        Name = "Minecraft",
        Description = "A sandbox game where players explore, build, and survive in a procedurally generated world.",
        Pegi = Pegi.Pegi7,
        Genre = Genre.Sandbox,
        ImageUrl = "https://example.com/images/minecraft.jpg",
        DeveloperId = DeveloperSeeds.Mojang.Id,
        Developer = DeveloperSeeds.Mojang
    };

    public static readonly GameEntity Portal2 = new()
    {
        Id = Guid.Parse("20000000-0000-0000-0000-000000000006"),
        Name = "Portal 2",
        Description = "A puzzle-platform game where players use a portal gun to solve increasingly complex challenges.",
        Pegi = Pegi.Pegi12,
        Genre = Genre.Puzzle,
        ImageUrl = "https://example.com/images/portal2.jpg",
        DeveloperId = DeveloperSeeds.Valve.Id,
        Developer = DeveloperSeeds.Valve
    };

    public static DbContext SeedGames(this DbContext dbx)
    {
        dbx.Set<GameEntity>().AddRange(
            CS2 with { Developer = null! },
            Witcher3 with { Developer = null! },
            GTA5 with { Developer = null! },
            LastOfUs with { Developer = null! },
            Minecraft with { Developer = null! },
            Portal2 with { Developer = null! }
        );

        return dbx;
    }
}