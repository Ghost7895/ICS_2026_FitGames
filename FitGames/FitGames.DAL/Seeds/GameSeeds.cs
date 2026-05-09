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

    public static readonly GameEntity Prey = new()
    {
        Id = Guid.Parse("20000000-0000-0000-0000-000000000007"),
        Name = "Prey",
        Description = "A survival horror where you wake up on alien swarmed space station and try to find out what happened.",
        Pegi = Pegi.Pegi18,
        Genre = Genre.Horror,
        ImageUrl = "https://cdn-ext.fanatical.com/production/product/1280x720/6e50825e-97f9-4d4f-9f48-a42fd05c5a16.jpg",
        DeveloperId = DeveloperSeeds.Arkane.Id,
        Developer = DeveloperSeeds.Arkane
    };

    public static readonly GameEntity REVillage = new()
    {
        Id = Guid.Parse("20000000-0000-0000-0000-000000000008"),
        Name = "Resident Evil Village",
        Description = "A survival horror where you try to save your daughter from cursed village.",
        Pegi = Pegi.Pegi18,
        Genre = Genre.Horror,
        ImageUrl = "https://www.relyonhorror.com/wp-content/uploads/2021/05/share.png",
        DeveloperId = DeveloperSeeds.Capcom.Id,
        Developer = DeveloperSeeds.Capcom
    };

    public static readonly GameEntity MousePI = new()
    {
        Id = Guid.Parse("20000000-0000-0000-0000-000000000009"),
        Name = "MOUSE: P.I. For Hire",
        Description = "An action cartoon where you play as a private investigator cleansing world from corruption.",
        Pegi = Pegi.Pegi16,
        Genre = Genre.Action,
        ImageUrl = "https://assets.nintendo.com/image/upload/ar_16:9,c_lpad,w_1240/b_white/f_auto/q_auto/store/software/switch2/70010000101091/7277fab8f6588c4b4ed3ddbca23ba16ccbe7aaf5b2d82c371d95f86628d9df15",
        DeveloperId = DeveloperSeeds.Fumi.Id,
        Developer = DeveloperSeeds.Fumi
    };

    public static readonly GameEntity RDR2 = new()
    {
        Id = Guid.Parse("20000000-0000-0000-0000-000000000010"),
        Name = "Red Dead Redemption 2",
        Description = "An action western where you play as an outlaw fighting for his live at the end of wild west era.",
        Pegi = Pegi.Pegi18,
        Genre = Genre.Action,
        ImageUrl = "https://www.tabletowo.pl/wp-content/uploads/2018/11/red-dead-redemption.jpg",
        DeveloperId = DeveloperSeeds.Rockstar.Id,
        Developer = DeveloperSeeds.Rockstar
    };

    public static readonly GameEntity DarkSouls3 = new()
    {
        Id = Guid.Parse("20000000-0000-0000-0000-000000000011"),
        Name = "DARK SOULS™ III",
        Description = "A dark fantasy that takes place in fallen kingdom where age of fire is coming to an end.",
        Pegi = Pegi.Pegi18,
        Genre = Genre.Fighting,
        ImageUrl = "https://img-cdn.heureka.group/v1/3c9c5eba-971b-43d3-9f55-04be52054777.jpg",
        DeveloperId = DeveloperSeeds.FromSoft.Id,
        Developer = DeveloperSeeds.FromSoft
    };

    public static readonly GameEntity MetroExodus = new()
    {
        Id = Guid.Parse("20000000-0000-0000-0000-000000000012"),
        Name = "Metro Exodus",
        Description = "An action post-apocalyptic survival horror that takes place in Russian wilderness.",
        Pegi = Pegi.Pegi18,
        Genre = Genre.Action,
        ImageUrl = "https://www.gamehype.co.uk/wp-content/uploads/2019/03/Game-Hype-Metro-Exodus-1-1280x640.jpg",
        DeveloperId = DeveloperSeeds.FourAGames.Id,
        Developer = DeveloperSeeds.FourAGames
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