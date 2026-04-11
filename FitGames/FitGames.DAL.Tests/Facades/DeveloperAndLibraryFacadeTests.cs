using FitGames.BL.Facades;
using FitGames.BL.Mappers;
using FitGames.DAL.Entities;
using FitGames.DAL.Factories;
using FitGames.DAL.Mappers;
using FitGames.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Tests.Facades;

public class DeveloperFacadeTests : DbContextTestsBase
{
    private DeveloperFacade CreateDeveloperFacade()
    {
        var unitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
        var modelMapper = new DeveloperModelMapper();
        return new DeveloperFacade(unitOfWorkFactory, modelMapper);
    }

    private async Task<DeveloperEntity> CreateAndSaveDeveloperEntity(string name = "Test Developer")
    {
        var developer = new DeveloperEntity { Name = name };
        GameDbContextSut.Developers.Add(developer);
        await GameDbContextSut.SaveChangesAsync();
        return developer;
    }

    [Fact]
    public async Task GetAsync_ReturnsAllDevelopers()
    {
        // Arrange
        await CreateAndSaveDeveloperEntity("Developer 1");
        await CreateAndSaveDeveloperEntity("Developer 2");
        var facade = CreateDeveloperFacade();

        // Act
        var result = await facade.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAsync_WithId_ReturnsDeveloperDetail()
    {
        // Arrange
        var developer = await CreateAndSaveDeveloperEntity("Rockstar Games");
        var facade = CreateDeveloperFacade();

        // Act
        var result = await facade.GetAsync(developer.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Rockstar Games", result.Name);
    }

    [Fact]
    public async Task GetAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var facade = CreateDeveloperFacade();
        var invalidId = Guid.NewGuid();

        // Act
        var result = await facade.GetAsync(invalidId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SaveAsync_CreatesNewDeveloper()
    {
        // Arrange
        var facade = CreateDeveloperFacade();
        var developerModel = new BL.Models.DeveloperDetailModel
        {
            Id = Guid.NewGuid(),
            Name = "New Studio"
        };

        // Act
        var result = await facade.SaveAsync(developerModel);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);

        // Verify in database
        await using var dbx = DbContextFactory.CreateDbContext();
        var savedDev = await dbx.Developers.FirstOrDefaultAsync(d => d.Id == result.Id);
        Assert.NotNull(savedDev);
        Assert.Equal("New Studio", savedDev.Name);
    }

    [Fact]
    public async Task SaveAsync_UpdatesExistingDeveloper()
    {
        // Arrange
        var original = await CreateAndSaveDeveloperEntity("Original Name");
        var facade = CreateDeveloperFacade();
        var updatedModel = new BL.Models.DeveloperDetailModel
        {
            Id = original.Id,
            Name = "Updated Studio"
        };

        // Act
        var result = await facade.SaveAsync(updatedModel);

        // Assert
        Assert.Equal("Updated Studio", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_RemovesDeveloper()
    {
        // Arrange
        var developer = await CreateAndSaveDeveloperEntity();
        var facade = CreateDeveloperFacade();

        // Act
        await facade.DeleteAsync(developer.Id);

        // Assert
        await using var dbx = DbContextFactory.CreateDbContext();
        var deleted = await dbx.Developers.FirstOrDefaultAsync(d => d.Id == developer.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task SaveAsync_WithDuplicateName_UpdatesExisting()
    {
        // Arrange
        var original = await CreateAndSaveDeveloperEntity("Duplicate Name");
        var facade = CreateDeveloperFacade();
        var model = new BL.Models.DeveloperDetailModel
        {
            Id = original.Id,
            Name = "Duplicate Name"
        };

        // Act & Assert
        var result = await facade.SaveAsync(model);
        Assert.Equal("Duplicate Name", result.Name);
    }
}

public class LibraryFacadeTests : DbContextTestsBase
{
    private LibraryFacade CreateLibraryFacade()
    {
        var unitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
        var modelMapper = new LibraryModelMapper();
        return new LibraryFacade(unitOfWorkFactory, modelMapper);
    }

    private async Task<LibraryEntity> CreateAndSaveLibraryEntity(string name = "My Library")
    {
        var library = new LibraryEntity { Name = name };
        GameDbContextSut.Libraries.Add(library);
        await GameDbContextSut.SaveChangesAsync();
        return library;
    }

    [Fact]
    public async Task GetAsync_ReturnsAllLibraries()
    {
        // Arrange
        await CreateAndSaveLibraryEntity("Library 1");
        await CreateAndSaveLibraryEntity("Library 2");
        var facade = CreateLibraryFacade();

        // Act
        var result = await facade.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAsync_WithId_ReturnsLibraryDetail()
    {
        // Arrange
        var library = await CreateAndSaveLibraryEntity("My Game Collection");
        var facade = CreateLibraryFacade();

        // Act
        var result = await facade.GetAsync(library.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("My Game Collection", result.Name);
    }

    [Fact]
    public async Task GetAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var facade = CreateLibraryFacade();

        // Act
        var result = await facade.GetAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SaveAsync_CreatesNewLibrary()
    {
        // Arrange
        var facade = CreateLibraryFacade();
        var libraryModel = new BL.Models.LibraryDetailModel
        {
            Id = Guid.NewGuid(),
            Name = "New Collection"
        };

        // Act
        var result = await facade.SaveAsync(libraryModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Collection", result.Name);

        // Verify in database
        await using var dbx = DbContextFactory.CreateDbContext();
        var saved = await dbx.Libraries.FirstOrDefaultAsync(l => l.Id == result.Id);
        Assert.NotNull(saved);
    }

    [Fact]
    public async Task SaveAsync_UpdatesExistingLibrary()
    {
        // Arrange
        var original = await CreateAndSaveLibraryEntity("Original Name");
        var facade = CreateLibraryFacade();
        var updated = new BL.Models.LibraryDetailModel
        {
            Id = original.Id,
            Name = "Updated Collection"
        };

        // Act
        var result = await facade.SaveAsync(updated);

        // Assert
        Assert.Equal("Updated Collection", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_RemovesLibrary()
    {
        // Arrange
        var library = await CreateAndSaveLibraryEntity();
        var facade = CreateLibraryFacade();

        // Act
        await facade.DeleteAsync(library.Id);

        // Assert
        await using var dbx = DbContextFactory.CreateDbContext();
        var deleted = await dbx.Libraries.FirstOrDefaultAsync(l => l.Id == library.Id);
        Assert.Null(deleted);
    }
}
