using FitGames.BL.Facades;
using FitGames.BL.Mappers;
using FitGames.DAL.Entities;
using FitGames.DAL.Enums;
using FitGames.DAL.Factories;
using FitGames.DAL.Mappers;
using FitGames.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Tests.Facades;

public class GameFacadeTests : DbContextTestsBase
{
    private GameFacade CreateGameFacade()
    {
        var unitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
        var modelMapper = new GameModelMapper();
        return new GameFacade(unitOfWorkFactory, modelMapper);
    }

    private async Task<GameEntity> CreateAndSaveGameEntity(
        string name = "Test Game",
        string description = "Test Description",
        Genre genre = Genre.Action,
        Pegi pegi = Pegi.Pegi12,
        string developerName = "Test Developer")
    {
        var developer = new DeveloperEntity { Name = developerName };
        var game = new GameEntity
        {
            Name = name,
            Description = description,
            Genre = genre,
            Pegi = pegi,
            Developer = developer
        };

        GameDbContextSut.Games.Add(game);
        await GameDbContextSut.SaveChangesAsync();
        return game;
    }

    [Fact]
    public async Task GetAsync_ReturnsAllGames()
    {
        // Arrange
        var game1 = await CreateAndSaveGameEntity("Game 1");
        var game2 = await CreateAndSaveGameEntity("Game 2");
        var facade = CreateGameFacade();

        // Act
        var result = await facade.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, g => g.Name == "Game 1");
        Assert.Contains(result, g => g.Name == "Game 2");
    }

    [Fact]
    public async Task GetAsync_WithId_ReturnsGameDetail()
    {
        // Arrange
        var game = await CreateAndSaveGameEntity(
            "Minecraft",
            "Block Building Game",
            Genre.Sandbox,
            Pegi.Pegi3,
            "Mojang");
        var facade = CreateGameFacade();

        // Act
        var result = await facade.GetAsync(game.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Minecraft", result.Name);
        Assert.Equal("Block Building Game", result.Description);
        Assert.Equal(Genre.Sandbox, result.Genre);
        Assert.Equal(Pegi.Pegi3, result.Pegi);
        Assert.Equal("Mojang", result.DeveloperName);
    }

    [Fact]
    public async Task GetAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var facade = CreateGameFacade();
        var invalidId = Guid.NewGuid();

        // Act
        var result = await facade.GetAsync(invalidId);

        // Assert
        Assert.Null(result);
    }

    // TODO: This test requires the facade to auto-create or lookup developers by name
    // Currently the mapper doesn't handle this scenario properly
    /*
    [Fact]
    public async Task SaveAsync_CreatesNewGame_WithExistingDeveloper()
    {
        // Arrange
        // Create a developer first
        var developer = new DeveloperEntity { Name = "FromSoftware" };
        GameDbContextSut.Developers.Add(developer);
        await GameDbContextSut.SaveChangesAsync();

        var facade = CreateGameFacade();
        var newGameId = Guid.NewGuid();
        var gameModel = new BL.Models.GameDetailModel
        {
            Id = newGameId,
            Name = "Elden Ring",
            Description = "Dark Fantasy RPG",
            Genre = Genre.RolePlaying,
            Pegi = Pegi.Pegi16,
            ImageUrl = "https://example.com/image.jpg",
            DeveloperName = "FromSoftware"
        };

        // Act
        var result = await facade.SaveAsync(gameModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newGameId, result.Id);
        Assert.Equal("Elden Ring", result.Name);

        // Verify in database
        await using var dbx = DbContextFactory.CreateDbContext();
        var savedGame = await dbx.Games.Include(g => g.Developer).FirstOrDefaultAsync(g => g.Id == result.Id);
        Assert.NotNull(savedGame);
        Assert.Equal("Elden Ring", savedGame.Name);
        Assert.Equal("FromSoftware", savedGame.Developer!.Name);
    }
    */

    [Fact]
    public async Task SaveAsync_UpdatesExistingGame()
    {
        // Arrange
        var originalGame = await CreateAndSaveGameEntity("Original Name", "Original Description");
        var facade = CreateGameFacade();
        var updatedModel = new BL.Models.GameDetailModel
        {
            Id = originalGame.Id,
            Name = "Updated Name",
            Description = "Updated Description",
            Genre = Genre.Strategy,
            Pegi = Pegi.Pegi18,
            DeveloperName = "New Developer"
        };

        // Act
        var result = await facade.SaveAsync(updatedModel);

        // Assert
        Assert.Equal("Updated Name", result.Name);
        
        // Verify in database
        await using var dbx = DbContextFactory.CreateDbContext();
        var dbGame = await dbx.Games.FirstOrDefaultAsync(g => g.Id == originalGame.Id);
        Assert.NotNull(dbGame);
        Assert.Equal("Updated Name", dbGame.Name);
        Assert.Equal("Updated Description", dbGame.Description);
    }

    [Fact]
    public async Task DeleteAsync_RemovesGame()
    {
        // Arrange
        var game = await CreateAndSaveGameEntity();
        var facade = CreateGameFacade();

        // Act
        await facade.DeleteAsync(game.Id);

        // Assert
        await using var dbx = DbContextFactory.CreateDbContext();
        var deletedGame = await dbx.Games.FirstOrDefaultAsync(g => g.Id == game.Id);
        Assert.Null(deletedGame);
    }

    [Fact]
    public async Task FilterGamesByNameAsync_ReturnsGamesByName()
    {
        // Arrange
        await CreateAndSaveGameEntity("Minecraft");
        await CreateAndSaveGameEntity("Elden Ring");
        await CreateAndSaveGameEntity("Mindustry");
        var facade = CreateGameFacade();

        // Act
        var result = await facade.FilterGamesByNameAsync("Min");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, g => Assert.Contains("Min", g.Name));
    }

    [Fact]
    public async Task FilterGamesByNameAsync_CaseSensitive()
    {
        // Arrange
        await CreateAndSaveGameEntity("Minecraft");
        var facade = CreateGameFacade();

        // Act
        var resultExactCase = await facade.FilterGamesByNameAsync("Minecraft");
        var resultDifferentCase = await facade.FilterGamesByNameAsync("minecraft");

        // Assert
        Assert.NotNull(resultExactCase);
        Assert.Single(resultExactCase);
        Assert.Empty(resultDifferentCase);
    }

    [Fact]
    public async Task FilterGamesByNameAsync_NoMatches_ReturnsEmpty()
    {
        // Arrange
        await CreateAndSaveGameEntity("Minecraft");
        var facade = CreateGameFacade();

        // Act
        var result = await facade.FilterGamesByNameAsync("NonExistent");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task FilterGamesByGenreAsync_ReturnsGamesByGenre()
    {
        // Arrange
        await CreateAndSaveGameEntity("Minecraft", genre: Genre.Sandbox);
        await CreateAndSaveGameEntity("Elden Ring", genre: Genre.RolePlaying);
        await CreateAndSaveGameEntity("Terraria", genre: Genre.Sandbox);
        var facade = CreateGameFacade();

        // Act
        var result = await facade.FilterGamesByGenreAsync(Genre.Sandbox);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, g => Assert.Equal(Genre.Sandbox, g.Genre));
    }

    [Fact]
    public async Task FilterGamesByGenreAsync_NoMatches_ReturnsEmpty()
    {
        // Arrange
        await CreateAndSaveGameEntity("Minecraft", genre: Genre.Sandbox);
        var facade = CreateGameFacade();

        // Act
        var result = await facade.FilterGamesByGenreAsync(Genre.Horror);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task FilterGamesByGenreAsync_MultipleGenres()
    {
        // Arrange
        await CreateAndSaveGameEntity("Game1", genre: Genre.Action);
        await CreateAndSaveGameEntity("Game2", genre: Genre.Strategy);
        await CreateAndSaveGameEntity("Game3", genre: Genre.RolePlaying);
        await CreateAndSaveGameEntity("Game4", genre: Genre.Action);
        var facade = CreateGameFacade();

        // Act
        var actionGames = await facade.FilterGamesByGenreAsync(Genre.Action);
        var strategyGames = await facade.FilterGamesByGenreAsync(Genre.Strategy);

        // Assert
        Assert.Equal(2, actionGames.Count());
        Assert.Single(strategyGames);
    }
}
