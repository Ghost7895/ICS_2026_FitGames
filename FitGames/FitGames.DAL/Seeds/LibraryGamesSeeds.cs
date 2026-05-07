using FitGames.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Seeds;

public static class LibraryGameSeeds
{
    // --- Alice's Library ---
    public static readonly LibraryGameEntity AliceCS2 = new()
    {
        LibraryId = LibrarySeeds.AlicesLibrary.Id,
        GameId = GameSeeds.CS2.Id,
        Library = LibrarySeeds.AlicesLibrary,
        Game = GameSeeds.CS2
    };

    public static readonly LibraryGameEntity AliceWitcher3 = new()
    {
        LibraryId = LibrarySeeds.AlicesLibrary.Id,
        GameId = GameSeeds.Witcher3.Id,
        Library = LibrarySeeds.AlicesLibrary,
        Game = GameSeeds.Witcher3
    };

    public static readonly LibraryGameEntity AliceMinecraft = new()
    {
        LibraryId = LibrarySeeds.AlicesLibrary.Id,
        GameId = GameSeeds.Minecraft.Id,
        Library = LibrarySeeds.AlicesLibrary,
        Game = GameSeeds.Minecraft
    };

    // --- Bob's Library ---
    public static readonly LibraryGameEntity BobGTA5 = new()
    {
        LibraryId = LibrarySeeds.BobsLibrary.Id,
        GameId = GameSeeds.GTA5.Id,
        Library = LibrarySeeds.BobsLibrary,
        Game = GameSeeds.GTA5
    };

    public static readonly LibraryGameEntity BobLastOfUs = new()
    {
        LibraryId = LibrarySeeds.BobsLibrary.Id,
        GameId = GameSeeds.LastOfUs.Id,
        Library = LibrarySeeds.BobsLibrary,
        Game = GameSeeds.LastOfUs
    };

    public static readonly LibraryGameEntity BobPortal2 = new()
    {
        LibraryId = LibrarySeeds.BobsLibrary.Id,
        GameId = GameSeeds.Portal2.Id,
        Library = LibrarySeeds.BobsLibrary,
        Game = GameSeeds.Portal2
    };

    // --- Shared/Public Library ---
    public static readonly LibraryGameEntity SharedWitcher3 = new()
    {
        LibraryId = LibrarySeeds.SharedLibrary.Id,
        GameId = GameSeeds.Witcher3.Id,
        Library = LibrarySeeds.SharedLibrary,
        Game = GameSeeds.Witcher3
    };

    public static readonly LibraryGameEntity SharedMinecraft = new()
    {
        LibraryId = LibrarySeeds.SharedLibrary.Id,
        GameId = GameSeeds.Minecraft.Id,
        Library = LibrarySeeds.SharedLibrary,
        Game = GameSeeds.Minecraft
    };

    public static readonly LibraryGameEntity SharedPortal2 = new()
    {
        LibraryId = LibrarySeeds.SharedLibrary.Id,
        GameId = GameSeeds.Portal2.Id,
        Library = LibrarySeeds.SharedLibrary,
        Game = GameSeeds.Portal2
    };

    public static DbContext SeedLibraryGames(this DbContext dbx)
    {
        dbx.Set<LibraryGameEntity>().AddRange(
            AliceCS2 with { Library = null!, Game = null! },
            AliceWitcher3 with { Library = null!, Game = null! },
            AliceMinecraft with { Library = null!, Game = null! },
            BobGTA5 with { Library = null!, Game = null! },
            BobLastOfUs with { Library = null!, Game = null! },
            BobPortal2 with { Library = null!, Game = null! },
            SharedWitcher3 with { Library = null!, Game = null! },
            SharedMinecraft with { Library = null!, Game = null! },
            SharedPortal2 with { Library = null!, Game = null! }
        );

        return dbx;
    }
}