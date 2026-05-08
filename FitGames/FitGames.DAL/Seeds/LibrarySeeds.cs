using FitGames.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Seeds;

public static class LibrarySeeds
{
    public static readonly LibraryEntity AlicesLibrary = new()
    {
        Id = Guid.Parse("30000000-0000-0000-0000-000000000001"),
        Name = "Alice's Collection",
        UserId = Guid.Parse("40000000-0000-0000-0000-000000000001") // UserSeeds.Alice.Id
    };

    public static readonly LibraryEntity BobsLibrary = new()
    {
        Id = Guid.Parse("30000000-0000-0000-0000-000000000002"),
        Name = "Bob's Favourites",
        UserId = Guid.Parse("40000000-0000-0000-0000-000000000002") // UserSeeds.Bob.Id
    };

    public static readonly LibraryEntity SharedLibrary = new()
    {
        Id = Guid.Parse("30000000-0000-0000-0000-000000000003"),
        Name = "Top Picks",
        UserId = null
    };

    public static DbContext SeedLibraries(this DbContext dbx)
    {
        dbx.Set<LibraryEntity>().AddRange(
            AlicesLibrary,
            BobsLibrary,
            SharedLibrary
        );

        return dbx;
    }
}