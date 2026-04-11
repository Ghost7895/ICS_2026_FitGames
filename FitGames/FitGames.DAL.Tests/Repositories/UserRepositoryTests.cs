using FitGames.DAL.Entities;
using FitGames.DAL.Mappers;
using FitGames.DAL.Repositories;

namespace FitGames.DAL.Tests.Repositories;

public class UserRepositoryTests : DbContextTestsBase
{
    private Repository<UserEntity> CreateUserRepository()
    {
        var mapper = new UserEntityMapper();
        return new Repository<UserEntity>(GameDbContextSut, mapper);
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
    public async Task Get_ReturnsAllUsers()
    {
        // Arrange
        var user1 = await CreateAndSaveUserEntity("user1", "user1@test.com");
        var user2 = await CreateAndSaveUserEntity("user2", "user2@test.com");
        var repository = CreateUserRepository();

        // Act
        var result = repository.Get().ToList();

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, u => u.Id == user1.Id && u.Username == "user1");
        Assert.Contains(result, u => u.Id == user2.Id && u.Username == "user2");
    }

    [Fact]
    public async Task Get_ReturnsEmptyWhenNoUsers()
    {
        // Arrange
        var repository = CreateUserRepository();

        // Act
        var result = repository.Get().ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Get_WithSearch_ReturnsMatchingUsers()
    {
        // Arrange
        await CreateAndSaveUserEntity("johndoe", "john@test.com");
        await CreateAndSaveUserEntity("janesmith", "jane@test.com");
        await CreateAndSaveUserEntity("bobwilson", "bob@test.com");
        var repository = CreateUserRepository();

        // Act
        var result = repository.Get()
            .Where(u => u.Username.Contains("john"))
            .ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("johndoe", result[0].Username);
    }

    [Fact]
    public async Task Get_SearchByEmail_ReturnsMatchingUser()
    {
        // Arrange
        await CreateAndSaveUserEntity("user1", "user1@gmail.com");
        await CreateAndSaveUserEntity("user2", "user2@yahoo.com");
        var repository = CreateUserRepository();

        // Act
        var result = repository.Get()
            .Where(u => u.Email.Contains("gmail"))
            .ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("user1@gmail.com", result[0].Email);
    }

    [Fact]
    public async Task Get_OrderedByUsername_ReturnsSortedUsers()
    {
        // Arrange
        await CreateAndSaveUserEntity("zebra");
        await CreateAndSaveUserEntity("apple");
        await CreateAndSaveUserEntity("monkey");
        var repository = CreateUserRepository();

        // Act
        var result = repository.Get()
            .OrderBy(u => u.Username)
            .ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("apple", result[0].Username);
        Assert.Equal("monkey", result[1].Username);
        Assert.Equal("zebra", result[2].Username);
    }

    [Fact]
    public async Task Get_OrderedByEmailDescending_ReturnsSortedUsers()
    {
        // Arrange
        await CreateAndSaveUserEntity("user1", "aaa@test.com");
        await CreateAndSaveUserEntity("user2", "zzz@test.com");
        await CreateAndSaveUserEntity("user3", "mmm@test.com");
        var repository = CreateUserRepository();

        // Act
        var result = repository.Get()
            .OrderByDescending(u => u.Email)
            .ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.StartsWith("zzz", result[0].Email);
        Assert.StartsWith("mmm", result[1].Email);
        Assert.StartsWith("aaa", result[2].Email);
    }

    [Fact]
    public async Task Get_WithPaging_ReturnsPagedUsers()
    {
        // Arrange
        for (int i = 1; i <= 10; i++)
        {
            await CreateAndSaveUserEntity($"user{i:00}", $"user{i}@test.com");
        }

        var repository = CreateUserRepository();
        int pageSize = 3;
        int pageNumber = 2;

        // Act
        var result = repository.Get()
            .OrderBy(u => u.Username)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task Insert_AddsUserToRepository()
    {
        // Arrange
        var repository = CreateUserRepository();
        var library = new LibraryEntity { Name = "New User's Library" };
        GameDbContextSut.Libraries.Add(library);
        await GameDbContextSut.SaveChangesAsync();

        var newUser = new UserEntity
        {
            Username = "newuser",
            Email = "new@test.com",
            Name = "New",
            Surname = "User",
            PhoneNumber = "9876543210",
            Library = library
        };

        // Act
        var result = repository.Insert(newUser);
        await GameDbContextSut.SaveChangesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);

        // Verify in database
        var userFromDb = GameDbContextSut.Users.FirstOrDefault(u => u.Id == result.Id);
        Assert.NotNull(userFromDb);
        Assert.Equal("newuser", userFromDb.Username);
    }

    [Fact]
    public async Task ExistAsync_ReturnsTrueForExistingUser()
    {
        // Arrange
        var user = await CreateAndSaveUserEntity();
        var repository = CreateUserRepository();

        // Act
        var result = await repository.ExistAsync(user);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistAsync_ReturnsFalseForNonExistingUser()
    {
        // Arrange
        var repository = CreateUserRepository();
        var library = new LibraryEntity { Name = "Temp Library" };
        GameDbContextSut.Libraries.Add(library);
        await GameDbContextSut.SaveChangesAsync();

        var nonExistentUser = new UserEntity
        {
            Id = Guid.NewGuid(),
            Username = "nonexistent",
            Email = "nonexistent@test.com",
            Name = "Non",
            Surname = "Existent",
            Library = library
        };

        // Act
        var result = await repository.ExistAsync(nonExistentUser);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingUser()
    {
        // Arrange
        var originalUser = await CreateAndSaveUserEntity("original", "original@test.com", "Old", "Name");
        var repository = CreateUserRepository();

        var updatedUser = new UserEntity
        {
            Id = originalUser.Id,
            Username = "original",
            Email = "updated@test.com",
            Name = "Updated",
            Surname = "Name",
            PhoneNumber = "9999999999",
            Library = originalUser.Library
        };

        // Act
        var result = await repository.UpdateAsync(updatedUser);
        await GameDbContextSut.SaveChangesAsync();

        // Assert
        Assert.Equal("updated@test.com", result.Email);
        Assert.Equal("Updated", result.Name);

        // Verify in database
        var userFromDb = GameDbContextSut.Users.FirstOrDefault(u => u.Id == originalUser.Id);
        Assert.NotNull(userFromDb);
        Assert.Equal("updated@test.com", userFromDb.Email);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsExceptionForNonExistentUser()
    {
        // Arrange
        var repository = CreateUserRepository();
        var library = new LibraryEntity { Name = "Temp Library" };
        GameDbContextSut.Libraries.Add(library);
        await GameDbContextSut.SaveChangesAsync();

        var nonExistentUser = new UserEntity
        {
            Id = Guid.NewGuid(),
            Username = "nonexistent",
            Email = "nonexistent@test.com",
            Name = "Non",
            Surname = "Existent",
            Library = library
        };

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => repository.UpdateAsync(nonExistentUser));
    }

    [Fact]
    public async Task DeleteAsync_RemovesUserFromRepository()
    {
        // Arrange
        var user = await CreateAndSaveUserEntity();
        var repository = CreateUserRepository();

        // Act
        await repository.DeleteAsync(user.Id);
        await GameDbContextSut.SaveChangesAsync();

        // Assert
        var userFromDb = GameDbContextSut.Users.FirstOrDefault(u => u.Id == user.Id);
        Assert.Null(userFromDb);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsExceptionForNonExistentUser()
    {
        // Arrange
        var repository = CreateUserRepository();
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => repository.DeleteAsync(nonExistentId));
    }

    [Fact]
    public async Task Get_CaseSensitiveSearch_FindsExactMatches()
    {
        // Arrange
        await CreateAndSaveUserEntity("JohnDoe");
        await CreateAndSaveUserEntity("johndoe");
        var repository = CreateUserRepository();

        // Act
        var result = repository.Get()
            .Where(u => u.Username == "JohnDoe")
            .ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("JohnDoe", result[0].Username);
    }

    [Fact]
    public async Task Get_MultipleFilters_ReturnsFilteredUsers()
    {
        // Arrange
        await CreateAndSaveUserEntity("user1", "user1@gmail.com", "John");
        await CreateAndSaveUserEntity("user2", "user2@gmail.com", "Jane");
        await CreateAndSaveUserEntity("user3", "user3@yahoo.com", "John");
        var repository = CreateUserRepository();

        // Act
        var result = repository.Get()
            .Where(u => u.Name == "John" && u.Email.Contains("gmail"))
            .ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("user1", result[0].Username);
    }
}
