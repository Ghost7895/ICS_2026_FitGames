using FitGames.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity Alice = new()
    {
        Id = Guid.Parse("40000000-0000-0000-0000-000000000001"),
        Username = "alice_gamer",
        Email = "alice@fitgames.com",
        Name = "Alice",
        Surname = "Smith",
        PhoneNumber = "+420123456789",
        Library = LibrarySeeds.AlicesLibrary
    };

    public static readonly UserEntity Bob = new()
    {
        Id = Guid.Parse("40000000-0000-0000-0000-000000000002"),
        Username = "bob_plays",
        Email = "bob@fitgames.com",
        Name = "Bob",
        Surname = "Jones",
        PhoneNumber = null,
        Library = LibrarySeeds.BobsLibrary
    };

    public static DbContext SeedUsers(this DbContext dbx)
    {
        dbx.Set<UserEntity>().AddRange(
            Alice with { Library = null! },
            Bob with { Library = null! }
        );

        return dbx;
    }
}