using FitGames.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Seeds;

public static class DeveloperSeeds
{
    public static readonly DeveloperEntity Valve = new()
    {
        Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
        Name = "Valve Corporation"
    };

    public static readonly DeveloperEntity CDProjekt = new()
    {
        Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
        Name = "CD Projekt Red"
    };

    public static readonly DeveloperEntity Rockstar = new()
    {
        Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
        Name = "Rockstar Games"
    };

    public static readonly DeveloperEntity Naughty = new()
    {
        Id = Guid.Parse("10000000-0000-0000-0000-000000000004"),
        Name = "Naughty Dog"
    };

    public static readonly DeveloperEntity Mojang = new()
    {
        Id = Guid.Parse("10000000-0000-0000-0000-000000000005"),
        Name = "Mojang Studios"
    };

    public static DbContext SeedDevelopers(this DbContext dbx)
    {
        dbx.Set<DeveloperEntity>().AddRange(
            Valve,
            CDProjekt,
            Rockstar,
            Naughty,
            Mojang
        );

        return dbx;
    }
}