using FitGames.BL.Facades;
using FitGames.BL.Mappers;
using FitGames.DAL.Entities;
using FitGames.DAL.Factories;
using FitGames.DAL.Mappers;
using FitGames.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Tests.Facades;

public class UserFacadeTests : DbContextTestsBase
{
    private UserFacade CreateUserFacade()
    {
        var unitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
        var modelMapper = new UserModelMapper();
        return new UserFacade(unitOfWorkFactory, modelMapper);
    }

    private async Task<UserEntity> CreateAndSaveUserEntity(
        string username = "testuser",
        string email = "test@example.com",
        string name = "Test",
        string surname = "User",
        string phoneNumber = "1234567890")
    {
        var library = new LibraryEntity { Name = $"{username}'s Library" };
        var user = new UserEntity
        {
            Username = username,
            Email = email,
            Name = name,
            Surname = surname,
            PhoneNumber = phoneNumber,
            Library = library
        };

        GameDbContextSut.Users.Add(user);
        await GameDbContextSut.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task GetAsync_ReturnsAllUsers()
    {
        // Arrange
        await CreateAndSaveUserEntity("user1", "user1@test.com");
        await CreateAndSaveUserEntity("user2", "user2@test.com");
        var facade = CreateUserFacade();

        // Act
        var result = await facade.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAsync_WithId_ReturnsUserDetail()
    {
        // Arrange
        var user = await CreateAndSaveUserEntity(
            "johndoe",
            "john@example.com",
            "John",
            "Doe",
            "1234567890");
        var facade = CreateUserFacade();

        // Act
        var result = await facade.GetAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("johndoe", result.Username);
        Assert.Equal("john@example.com", result.Email);
        Assert.Equal("John", result.Name);
        Assert.Equal("Doe", result.Surname);
    }

    [Fact]
    public async Task GetAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var facade = CreateUserFacade();
        var invalidId = Guid.NewGuid();

        // Act
        var result = await facade.GetAsync(invalidId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SaveAsync_CreatesNewUser()
    {
        // Arrange
        var library = new LibraryEntity { Name = "New User's Library" };
        GameDbContextSut.Libraries.Add(library);
        await GameDbContextSut.SaveChangesAsync();
        
        var facade = CreateUserFacade();
        var userModel = new BL.Models.UserDetailModel
        {
            Id = Guid.NewGuid(),
            Username = "newuser",
            Email = "newuser@example.com",
            Name = "New",
            Surname = "User",
            PhoneNumber = "9876543210",
            LibraryId = library.Id
        };

        // Act
        var result = await facade.SaveAsync(userModel);

        // Assert
        Assert.NotNull(result);

        // Verify in database
        await using var dbx = DbContextFactory.CreateDbContext();
        var savedUser = await dbx.Users.FirstOrDefaultAsync(u => u.Id == result.Id);
        Assert.NotNull(savedUser);
        Assert.Equal("newuser", savedUser.Username);
    }

    [Fact]
    public async Task SaveAsync_UpdatesExistingUser()
    {
        // Arrange
        var originalUser = await CreateAndSaveUserEntity("original", "original@test.com");
        var facade = CreateUserFacade();
        var updatedModel = new BL.Models.UserDetailModel
        {
            Id = originalUser.Id,
            Username = "updated",
            Email = "updated@example.com",
            Name = "Updated",
            Surname = "Name",
            PhoneNumber = "5555555555",
            LibraryId = originalUser.Library!.Id
        };

        // Act
        var result = await facade.SaveAsync(updatedModel);

        // Assert
        Assert.Equal("updated", result.Username);

        // Verify in database
        await using var dbx = DbContextFactory.CreateDbContext();
        var dbUser = await dbx.Users.FirstOrDefaultAsync(u => u.Id == originalUser.Id);
        Assert.NotNull(dbUser);
        Assert.Equal("updated", dbUser.Username);
    }

    [Fact]
    public async Task DeleteAsync_RemovesUser()
    {
        // Arrange
        var user = await CreateAndSaveUserEntity();
        var facade = CreateUserFacade();

        // Act
        await facade.DeleteAsync(user.Id);

        // Assert
        await using var dbx = DbContextFactory.CreateDbContext();
        var deletedUser = await dbx.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_ReturnsUserDetail()
    {
        // Arrange
        var user = await CreateAndSaveUserEntity(
            "targetuser",
            "target@example.com",
            "Target",
            "User");
        var facade = CreateUserFacade();

        // Act
        var result = await facade.GetUserByUsernameAsync("targetuser");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("targetuser", result.Username);
        Assert.Equal("target@example.com", result.Email);
        Assert.Equal("Target", result.Name);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_CaseInsensitive()
    {
        // Arrange
        await CreateAndSaveUserEntity("testuser");
        var facade = CreateUserFacade();

        // Act
        var resultLower = await facade.GetUserByUsernameAsync("testuser");
        var resultUpper = await facade.GetUserByUsernameAsync("TESTUSER");

        // Assert
        Assert.NotNull(resultLower);
        Assert.NotNull(resultUpper);
    }

    [Fact]
    public async Task SaveAsync_WithDuplicateUsername_UpdatesExisting()
    {
        // Arrange
        var existing = await CreateAndSaveUserEntity("duplicate", "first@example.com");
        var library = new LibraryEntity { Name = "Dup Library" };
        GameDbContextSut.Libraries.Add(library);
        await GameDbContextSut.SaveChangesAsync();

        var facade = CreateUserFacade();
        var userModel = new BL.Models.UserDetailModel
        {
            Id = existing.Id,
            Username = "duplicate",
            Email = "another@example.com",
            Name = "Updated",
            Surname = "User",
            LibraryId = library.Id
        };

        // Act
        var result = await facade.SaveAsync(userModel);

        // Assert
        Assert.Equal("another@example.com", result.Email);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_WithLibrary()
    {
        // Arrange
        var user = await CreateAndSaveUserEntity("gamer");
        var facade = CreateUserFacade();

        // Act
        var result = await facade.GetUserByUsernameAsync("gamer");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Library!.Id, result.LibraryId);
    }
}
