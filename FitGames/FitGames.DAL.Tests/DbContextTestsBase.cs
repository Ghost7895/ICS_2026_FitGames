using FitGames.DAL.Factories;

namespace FitGames.DAL.Tests;

public class DbContextTestsBase : IAsyncLifetime
{
    protected readonly DbContextSqLiteFactory DbContextFactory;
    protected readonly FitGamesDbContext GameDbContextSut;

    protected DbContextTestsBase()
    {
        DbContextFactory = new DbContextSqLiteFactory(GetType().FullName!);
        GameDbContextSut = DbContextFactory.CreateDbContext();
    }

    public async Task InitializeAsync() => await GameDbContextSut.Database.EnsureCreatedAsync();

    public async Task DisposeAsync()
    {
        await GameDbContextSut.Database.EnsureDeletedAsync();
        await GameDbContextSut.DisposeAsync();
    }
}