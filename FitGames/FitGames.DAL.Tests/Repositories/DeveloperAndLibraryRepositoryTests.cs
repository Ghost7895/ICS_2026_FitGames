using FitGames.DAL.Entities;
using FitGames.DAL.Mappers;
using FitGames.DAL.Repositories;

namespace FitGames.DAL.Tests.Repositories;

public class DeveloperRepositoryTests : DbContextTestsBase
{
    private Repository<DeveloperEntity> CreateDeveloperRepository()
    {
        var mapper = new DeveloperEntityMapper();
        return new Repository<DeveloperEntity>(GameDbContextSut, mapper);
    }

    private async Task<DeveloperEntity> CreateAndSaveDeveloperEntity(string name = "Test Developer")
    {
        var developer = new DeveloperEntity { Name = name };
        GameDbContextSut.Developers.Add(developer);
        await GameDbContextSut.SaveChangesAsync();
        return developer;
    }

    [Fact]
    public async Task Get_ReturnsAllDevelopers()
    {
        // Arrange
        var dev1 = await CreateAndSaveDeveloperEntity("Developer 1");
        var dev2 = await CreateAndSaveDeveloperEntity("Developer 2");
        var repository = CreateDeveloperRepository();

        // Act
        var result = repository.Get().ToList();

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, d => d.Id == dev1.Id && d.Name == "Developer 1");
        Assert.Contains(result, d => d.Id == dev2.Id && d.Name == "Developer 2");
    }

    [Fact]
    public async Task Get_WithSearch_ReturnsMatchingDevelopers()
    {
        // Arrange
        await CreateAndSaveDeveloperEntity("FromSoftware");
        await CreateAndSaveDeveloperEntity("Sony Interactive Entertainment");
        await CreateAndSaveDeveloperEntity("FromSoftware Japan");
        var repository = CreateDeveloperRepository();

        // Act
        var result = repository.Get()
            .Where(d => d.Name.Contains("FromSoftware"))
            .ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, d => Assert.Contains("FromSoftware", d.Name));
    }

    [Fact]
    public async Task Get_OrderedByName_ReturnsSortedDevelopers()
    {
        // Arrange
        await CreateAndSaveDeveloperEntity("Rockstar Games");
        await CreateAndSaveDeveloperEntity("Bethesda");
        await CreateAndSaveDeveloperEntity("Activision");
        var repository = CreateDeveloperRepository();

        // Act
        var result = repository.Get()
            .OrderBy(d => d.Name)
            .ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("Activision", result[0].Name);
        Assert.Equal("Bethesda", result[1].Name);
        Assert.Equal("Rockstar Games", result[2].Name);
    }

    [Fact]
    public async Task Get_WithPaging_ReturnsPagedDevelopers()
    {
        // Arrange
        for (int i = 1; i <= 10; i++)
        {
            await CreateAndSaveDeveloperEntity($"Dev {i:00}");
        }

        var repository = CreateDeveloperRepository();
        int pageSize = 3;
        int pageNumber = 2;

        // Act
        var result = repository.Get()
            .OrderBy(d => d.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task Insert_AddsDeveloperToRepository()
    {
        // Arrange
        var repository = CreateDeveloperRepository();
        var newDeveloper = new DeveloperEntity { Name = "New Studio" };

        // Act
        var result = repository.Insert(newDeveloper);
        await GameDbContextSut.SaveChangesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);

        // Verify in database
        var devFromDb = GameDbContextSut.Developers.FirstOrDefault(d => d.Id == result.Id);
        Assert.NotNull(devFromDb);
        Assert.Equal("New Studio", devFromDb.Name);
    }

    [Fact]
    public async Task ExistAsync_ReturnsTrueForExistingDeveloper()
    {
        // Arrange
        var developer = await CreateAndSaveDeveloperEntity();
        var repository = CreateDeveloperRepository();

        // Act
        var result = await repository.ExistAsync(developer);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistAsync_ReturnsFalseForNonExistingDeveloper()
    {
        // Arrange
        var repository = CreateDeveloperRepository();
        var nonExistentDeveloper = new DeveloperEntity
        {
            Id = Guid.NewGuid(),
            Name = "Non Existent Studio"
        };

        // Act
        var result = await repository.ExistAsync(nonExistentDeveloper);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingDeveloper()
    {
        // Arrange
        var originalDev = await CreateAndSaveDeveloperEntity("Original Name");
        var repository = CreateDeveloperRepository();

        var updatedDeveloper = new DeveloperEntity
        {
            Id = originalDev.Id,
            Name = "Updated Studio Name"
        };

        // Act
        var result = await repository.UpdateAsync(updatedDeveloper);
        await GameDbContextSut.SaveChangesAsync();

        // Assert
        Assert.Equal("Updated Studio Name", result.Name);

        // Verify in database
        var devFromDb = GameDbContextSut.Developers.FirstOrDefault(d => d.Id == originalDev.Id);
        Assert.NotNull(devFromDb);
        Assert.Equal("Updated Studio Name", devFromDb.Name);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsExceptionForNonExistentDeveloper()
    {
        // Arrange
        var repository = CreateDeveloperRepository();
        var nonExistentDeveloper = new DeveloperEntity
        {
            Id = Guid.NewGuid(),
            Name = "Non Existent"
        };

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => repository.UpdateAsync(nonExistentDeveloper));
    }

    [Fact]
    public async Task DeleteAsync_RemovesDeveloperFromRepository()
    {
        // Arrange
        var developer = await CreateAndSaveDeveloperEntity();
        var repository = CreateDeveloperRepository();

        // Act
        await repository.DeleteAsync(developer.Id);
        await GameDbContextSut.SaveChangesAsync();

        // Assert
        var devFromDb = GameDbContextSut.Developers.FirstOrDefault(d => d.Id == developer.Id);
        Assert.Null(devFromDb);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsExceptionForNonExistentDeveloper()
    {
        // Arrange
        var repository = CreateDeveloperRepository();
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => repository.DeleteAsync(nonExistentId));
    }

    [Fact]
    public async Task Get_CaseSensitiveSearch_FindsExactMatches()
    {
        // Arrange
        await CreateAndSaveDeveloperEntity("FromSoftware");
        await CreateAndSaveDeveloperEntity("fromsoftware");
        var repository = CreateDeveloperRepository();

        // Act
        var result = repository.Get()
            .Where(d => d.Name == "FromSoftware")
            .ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("FromSoftware", result[0].Name);
    }
}

public class LibraryRepositoryTests : DbContextTestsBase
{
    private Repository<LibraryEntity> CreateLibraryRepository()
    {
        var mapper = new LibraryEntityMapper();
        return new Repository<LibraryEntity>(GameDbContextSut, mapper);
    }

    private async Task<LibraryEntity> CreateAndSaveLibraryEntity(string name = "Test Library")
    {
        var library = new LibraryEntity { Name = name };
        GameDbContextSut.Libraries.Add(library);
        await GameDbContextSut.SaveChangesAsync();
        return library;
    }

    [Fact]
    public async Task Get_ReturnsAllLibraries()
    {
        // Arrange
        var lib1 = await CreateAndSaveLibraryEntity("Library 1");
        var lib2 = await CreateAndSaveLibraryEntity("Library 2");
        var repository = CreateLibraryRepository();

        // Act
        var result = repository.Get().ToList();

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, l => l.Id == lib1.Id && l.Name == "Library 1");
        Assert.Contains(result, l => l.Id == lib2.Id && l.Name == "Library 2");
    }

    [Fact]
    public async Task Get_WithSearch_ReturnsMatchingLibraries()
    {
        // Arrange
        await CreateAndSaveLibraryEntity("My Collection");
        await CreateAndSaveLibraryEntity("Gaming Collection");
        await CreateAndSaveLibraryEntity("My Wishlist");
        var repository = CreateLibraryRepository();

        // Act
        var result = repository.Get()
            .Where(l => l.Name.Contains("Collection"))
            .ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, l => Assert.Contains("Collection", l.Name));
    }

    [Fact]
    public async Task Get_OrderedByName_ReturnsSortedLibraries()
    {
        // Arrange
        await CreateAndSaveLibraryEntity("Zebra Collection");
        await CreateAndSaveLibraryEntity("Apple Collection");
        await CreateAndSaveLibraryEntity("Monkey Collection");
        var repository = CreateLibraryRepository();

        // Act
        var result = repository.Get()
            .OrderBy(l => l.Name)
            .ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("Apple Collection", result[0].Name);
        Assert.Equal("Monkey Collection", result[1].Name);
        Assert.Equal("Zebra Collection", result[2].Name);
    }

    [Fact]
    public async Task Insert_AddsLibraryToRepository()
    {
        // Arrange
        var repository = CreateLibraryRepository();
        var newLibrary = new LibraryEntity { Name = "New Library" };

        // Act
        var result = repository.Insert(newLibrary);
        await GameDbContextSut.SaveChangesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);

        // Verify in database
        var libFromDb = GameDbContextSut.Libraries.FirstOrDefault(l => l.Id == result.Id);
        Assert.NotNull(libFromDb);
        Assert.Equal("New Library", libFromDb.Name);
    }

    [Fact]
    public async Task ExistAsync_ReturnsTrueForExistingLibrary()
    {
        // Arrange
        var library = await CreateAndSaveLibraryEntity();
        var repository = CreateLibraryRepository();

        // Act
        var result = await repository.ExistAsync(library);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingLibrary()
    {
        // Arrange
        var originalLib = await CreateAndSaveLibraryEntity("Original Name");
        var repository = CreateLibraryRepository();

        var updatedLibrary = new LibraryEntity
        {
            Id = originalLib.Id,
            Name = "Updated Name"
        };

        // Act
        var result = await repository.UpdateAsync(updatedLibrary);
        await GameDbContextSut.SaveChangesAsync();

        // Assert
        Assert.Equal("Updated Name", result.Name);

        // Verify in database
        var libFromDb = GameDbContextSut.Libraries.FirstOrDefault(l => l.Id == originalLib.Id);
        Assert.NotNull(libFromDb);
        Assert.Equal("Updated Name", libFromDb.Name);
    }

    [Fact]
    public async Task DeleteAsync_RemovesLibraryFromRepository()
    {
        // Arrange
        var library = await CreateAndSaveLibraryEntity();
        var repository = CreateLibraryRepository();

        // Act
        await repository.DeleteAsync(library.Id);
        await GameDbContextSut.SaveChangesAsync();

        // Assert
        var libFromDb = GameDbContextSut.Libraries.FirstOrDefault(l => l.Id == library.Id);
        Assert.Null(libFromDb);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsExceptionForNonExistentLibrary()
    {
        // Arrange
        var repository = CreateLibraryRepository();
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => repository.DeleteAsync(nonExistentId));
    }

    [Fact]
    public async Task Get_WithPaging_ReturnsPagedLibraries()
    {
        // Arrange
        for (int i = 1; i <= 10; i++)
        {
            await CreateAndSaveLibraryEntity($"Library {i:00}");
        }

        var repository = CreateLibraryRepository();
        int pageSize = 3;
        int pageNumber = 2;

        // Act
        var result = repository.Get()
            .OrderBy(l => l.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Assert
        Assert.Equal(3, result.Count);
    }
}
