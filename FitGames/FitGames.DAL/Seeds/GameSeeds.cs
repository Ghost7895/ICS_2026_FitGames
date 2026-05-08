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
        ImageUrl = "https://cdn.alza.cz/Foto/ImgGalery/Image/counter-strike-2-key-art_4.jpg",
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
        ImageUrl = "https://image.api.playstation.com/vulcan/ap/rnd/202211/0711/kh4MUIuMmHlktOHar3lVl6rY.png",
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
        ImageUrl = "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/3240220/4c8d7ce5142a528bdac68c093bd1bcc720e2baee/capsule_616x353.jpg?t=1765479644",
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
        ImageUrl = "https://cdn.alza.cz/Foto/ImgGalery/Image/the-last-of-us-part-1-key-art-ellie-nahled.jpg",
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
        ImageUrl = "https://cdn.alza.cz/Foto/ImgGalery/Image/minecraft-key-art.jpg",
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
        ImageUrl = "https://assets.nintendo.com/image/upload/q_auto/f_auto/store/software/switch/70010000050313/75484f73fedd25cb830c5d93fbb3fca643a5ec0b09df2815291ead880bc7d6b1",
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