using FitGames.DAL.Entities;
using FitGames.DAL.Enums;
using FitGames.DAL.Mappers;
using FitGames.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Tests.Repositories;

public class GameRepositoryTests : DbContextTestsBase
{
    private Repository<GameEntity> CreateGameRepository()
    {
        var mapper = new GameEntityMapper();
        return new Repository<GameEntity>(GameDbContextSut, mapper);
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
    public async Task Get_ReturnsAllGames()
    {
        // Arrange
        var game1 = await CreateAndSaveGameEntity("Game 1");
        var game2 = await CreateAndSaveGameEntity("Game 2");
        var repository = CreateGameRepository();

        // Act
        var result = repository.Get().ToList();

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, g => g.Id == game1.Id && g.Name == "Game 1");
        Assert.Contains(result, g => g.Id == game2.Id && g.Name == "Game 2");
    }

    [Fact]
    public async Task Get_ReturnsEmptyWhenNoGames()
    {
        // Arrange
        var repository = CreateGameRepository();

        // Act
        var result = repository.Get().ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Get_WithFilter_ReturnsFilteredGames()
    {
        // Arrange
        await CreateAndSaveGameEntity("Minecraft", genre: Genre.Sandbox);
        await CreateAndSaveGameEntity("Elden Ring", genre: Genre.RolePlaying);
        await CreateAndSaveGameEntity("Terraria", genre: Genre.Sandbox);
        var repository = CreateGameRepository();

        // Act
        var result = repository.Get()
            .Where(g => g.Genre == Genre.Sandbox)
            .ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, g => Assert.Equal(Genre.Sandbox, g.Genre));
    }

    [Fact]
    public async Task Get_WithSearch_ReturnsMatchingGames()
    {
        // Arrange
        await CreateAndSaveGameEntity("Minecraft");
        await CreateAndSaveGameEntity("Elden Ring");
        await CreateAndSaveGameEntity("Mindustry");
        var repository = CreateGameRepository();

        // Act
        var result = repository.Get()
            .Where(g => g.Name.Contains("Min"))
            .ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, g => Assert.Contains("Min", g.Name));
    }

    [Fact]
    public async Task Get_OrderedByName_ReturnsSortedGames()
    {
        // Arrange
        await CreateAndSaveGameEntity("Zebra Game");
        await CreateAndSaveGameEntity("Apple Game");
        await CreateAndSaveGameEntity("Monkey Game");
        var repository = CreateGameRepository();

        // Act
        var result = repository.Get()
            .OrderBy(g => g.Name)
            .ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("Apple Game", result[0].Name);
        Assert.Equal("Monkey Game", result[1].Name);
        Assert.Equal("Zebra Game", result[2].Name);
    }

    [Fact]
    public async Task Get_OrderedByGenreDescending_ReturnsSortedGames()
    {
        // Arrange
        await CreateAndSaveGameEntity("Game1", genre: Genre.Action);
        await CreateAndSaveGameEntity("Game2", genre: Genre.Strategy);
        await CreateAndSaveGameEntity("Game3", genre: Genre.RolePlaying);
        var repository = CreateGameRepository();

        // Act
        var result = repository.Get()
            .OrderByDescending(g => g.Genre)
            .ToList();

        // Assert
        Assert.Equal(3, result.Count);
        // Verify descending order by genre enum values
        for (int i = 0; i < result.Count - 1; i++)
        {
            Assert.True(result[i].Genre >= result[i + 1].Genre);
        }
    }

    [Fact]
    public async Task Get_WithPaging_ReturnsPagedGames()
    {
        // Arrange
        for (int i = 1; i <= 10; i++)
        {
            await CreateAndSaveGameEntity($"Game {i:00}");
        }

        var repository = CreateGameRepository();
        int pageSize = 3;
        int pageNumber = 2;

        // Act
        var result = repository.Get()
            .OrderBy(g => g.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Assert
        Assert.Equal(3, result.Count);
        // Verify we got page 2 (items 4-6)
        Assert.StartsWith("Game 04", result.First().Name);
    }

    [Fact]
    public async Task Insert_AddsGameToRepository()
    {
        // Arrange
        var repository = CreateGameRepository();
        var developer = new DeveloperEntity { Name = "New Dev" };
        GameDbContextSut.Developers.Add(developer);
        await GameDbContextSut.SaveChangesAsync();

        var newGame = new GameEntity
        {
            Name = "New Game",
            Description = "New Description",
            Genre = Genre.Action,
            Pegi = Pegi.Pegi12,
            Developer = developer
        };

        // Act
        var result = repository.Insert(newGame);
        await GameDbContextSut.SaveChangesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);

        // Verify in database
        var gameFromDb = GameDbContextSut.Games.FirstOrDefault(g => g.Id == result.Id);
        Assert.NotNull(gameFromDb);
        Assert.Equal("New Game", gameFromDb.Name);
    }

    [Fact]
    public async Task ExistAsync_ReturnsTrueForExistingGame()
    {
        // Arrange
        var game = await CreateAndSaveGameEntity();
        var repository = CreateGameRepository();

        // Act
        var result = await repository.ExistAsync(game);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistAsync_ReturnsFalseForNonExistingGame()
    {
        // Arrange
        var repository = CreateGameRepository();
        var dev = new DeveloperEntity { Name = "Test" };
        var nonExistentGame = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Non Existent",
            Description = "Doesn't exist",
            Genre = Genre.Action,
            Pegi = Pegi.Pegi12,
            Developer = dev
        };

        // Act
        var result = await repository.ExistAsync(nonExistentGame);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExistAsync_ReturnsFalseForGameWithEmptyId()
    {
        // Arrange
        var repository = CreateGameRepository();
        var dev = new DeveloperEntity { Name = "Test" };
        var gameWithEmptyId = new GameEntity
        {
            Id = Guid.Empty,
            Name = "Empty ID",
            Description = "Has empty ID",
            Genre = Genre.Action,
            Pegi = Pegi.Pegi12,
            Developer = dev
        };

        // Act
        var result = await repository.ExistAsync(gameWithEmptyId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingGame()
    {
        // Arrange
        var originalGame = await CreateAndSaveGameEntity("Original Name", "Original Desc");
        var repository = CreateGameRepository();

        var updatedGame = new GameEntity
        {
            Id = originalGame.Id,
            Name = "Updated Name",
            Description = "Updated Desc",
            Genre = Genre.Strategy,
            Pegi = Pegi.Pegi18,
            Developer = originalGame.Developer
        };

        // Act
        var result = await repository.UpdateAsync(updatedGame);
        await GameDbContextSut.SaveChangesAsync();

        // Assert
        Assert.Equal("Updated Name", result.Name);
        Assert.Equal("Updated Desc", result.Description);
        Assert.Equal(Genre.Strategy, result.Genre);

        // Verify in database
        var gameFromDb = GameDbContextSut.Games.FirstOrDefault(g => g.Id == originalGame.Id);
        Assert.NotNull(gameFromDb);
        Assert.Equal("Updated Name", gameFromDb.Name);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsExceptionForNonExistentGame()
    {
        // Arrange
        var repository = CreateGameRepository();
        var dev = new DeveloperEntity { Name = "Test" };
        var nonExistentGame = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Non Existent",
            Description = "Doesn't exist",
            Genre = Genre.Action,
            Pegi = Pegi.Pegi12,
            Developer = dev
        };

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => repository.UpdateAsync(nonExistentGame));
    }

    [Fact]
    public async Task DeleteAsync_RemovesGameFromRepository()
    {
        // Arrange
        var game = await CreateAndSaveGameEntity();
        var repository = CreateGameRepository();

        // Act
        await repository.DeleteAsync(game.Id);
        await GameDbContextSut.SaveChangesAsync();

        // Assert
        var gameFromDb = GameDbContextSut.Games.FirstOrDefault(g => g.Id == game.Id);
        Assert.Null(gameFromDb);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsExceptionForNonExistentGame()
    {
        // Arrange
        var repository = CreateGameRepository();
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => repository.DeleteAsync(nonExistentId));
    }

    [Fact]
    public async Task Get_WithMultipleFilters_ReturnsFilteredGames()
    {
        // Arrange
        await CreateAndSaveGameEntity("Game 1", genre: Genre.Action, pegi: Pegi.Pegi12);
        await CreateAndSaveGameEntity("Game 2", genre: Genre.Action, pegi: Pegi.Pegi18);
        await CreateAndSaveGameEntity("Game 3", genre: Genre.Strategy, pegi: Pegi.Pegi12);
        var repository = CreateGameRepository();

        // Act
        var result = repository.Get()
            .Where(g => g.Genre == Genre.Action && g.Pegi == Pegi.Pegi12)
            .ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Game 1", result[0].Name);
    }

    [Fact]
    public async Task Get_OrderByNameThenByGenre_ReturnsSortedGames()
    {
        // Arrange
        await CreateAndSaveGameEntity("B Game", genre: Genre.Strategy);
        await CreateAndSaveGameEntity("A Game", genre: Genre.Action);
        await CreateAndSaveGameEntity("A Game", genre: Genre.RolePlaying);
        var repository = CreateGameRepository();

        // Act
        var result = repository.Get()
            .OrderBy(g => g.Name)
            .ThenBy(g => g.Genre)
            .ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("A Game", result[0].Name);
        Assert.Equal("A Game", result[1].Name);
        Assert.Equal("B Game", result[2].Name);
    }
}
