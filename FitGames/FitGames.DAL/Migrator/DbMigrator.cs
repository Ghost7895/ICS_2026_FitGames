using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Migrator;

public class DbMigrator(IDbContextFactory<FitGamesDbContext> dbContextFactory) : IDbMigrator
{
    public void Migrate()
    {
        using FitGamesDbContext dbContext = dbContextFactory.CreateDbContext();
        dbContext.Database.Migrate();
    }
}