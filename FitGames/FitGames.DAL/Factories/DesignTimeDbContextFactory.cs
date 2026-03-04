using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FitGames.DAL.Factories;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FitGamesDbContext>
{
    public FitGamesDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FitGamesDbContext>();

        optionsBuilder.UseSqlite("Data Source=FitGames.db");

        return new FitGamesDbContext(optionsBuilder.Options);
    }
}
