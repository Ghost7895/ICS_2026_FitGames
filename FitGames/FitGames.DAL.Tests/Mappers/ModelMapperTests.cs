using FitGames.BL.Mappers;
using FitGames.BL.Models;
using FitGames.DAL.Entities;
using FitGames.DAL.Enums;

namespace FitGames.DAL.Tests.Mappers;

public class GameModelMapperTests
{
    private readonly GameModelMapper _mapper = new();

    [Fact]
    public void MapToListModel_WithValidEntity_ReturnsCorrectModel()
    {
        // Arrange
        var entity = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Test Game",
            ImageUrl = "https://example.com/image.jpg",
            Genre = Genre.Action,
            Description = "Description",
            Pegi = Pegi.Pegi12,
            Developer = new DeveloperEntity { Name = "Dev" }
        };

        // Act
        var result = _mapper.MapToListModel(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
        Assert.Equal("Test Game", result.Name);
        Assert.Equal("https://example.com/image.jpg", result.ImageUrl);
        Assert.Equal(Genre.Action, result.Genre);
    }

    [Fact]
    public void MapToListModel_WithoutOptionalFields_ReturnsModel()
    {
        // Arrange
        var entity = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Game",
            Genre = Genre.Action,
            Description = "Desc",
            Pegi = Pegi.Pegi12,
            ImageUrl = null,
            Developer = new DeveloperEntity { Name = "Dev" }
        };

        // Act
        var result = _mapper.MapToListModel(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
        Assert.Equal("Game", result.Name);
        Assert.Null(result.ImageUrl);
    }

    [Fact]
    public void MapToDetailModel_WithValidEntity_ReturnsCorrectModel()
    {
        // Arrange
        var entity = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Minecraft",
            Description = "Block Building Game",
            Genre = Genre.Sandbox,
            Pegi = Pegi.Pegi3,
            ImageUrl = "https://example.com/mc.jpg",
            Developer = new DeveloperEntity { Name = "Mojang" }
        };

        // Act
        var result = _mapper.MapToDetailModel(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Minecraft", result.Name);
        Assert.Equal("Block Building Game", result.Description);
        Assert.Equal(Genre.Sandbox, result.Genre);
        Assert.Equal(Pegi.Pegi3, result.Pegi);
        Assert.Equal("Mojang", result.DeveloperName);
    }

    [Fact]
    public void MapToDetailModel_WithNullDeveloper_ReturnsUnknown()
    {
        // Arrange
        var developer = new DeveloperEntity { Name = "Test Dev" };
        var entity = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Indie Game",
            Description = "Cool game",
            Genre = Genre.Puzzle,
            Pegi = Pegi.Pegi7,
            Developer = developer
        };

        // Act
        var result = _mapper.MapToDetailModel(entity);

        // Assert
        Assert.Equal("Test Dev", result.DeveloperName);
    }

    [Fact]
    public void MapToEntity_WithValidModel_ReturnsCorrectEntity()
    {
        // Arrange
        var model = new GameDetailModel
        {
            Id = Guid.NewGuid(),
            Name = "New Game",
            Description = "Game Description",
            Genre = Genre.RolePlaying,
            Pegi = Pegi.Pegi16,
            ImageUrl = "https://example.com/game.jpg"
        };

        // Act
        var result = _mapper.MapToEntity(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.Id, result.Id);
        Assert.Equal("New Game", result.Name);
        Assert.Equal("Game Description", result.Description);
        Assert.Equal(Genre.RolePlaying, result.Genre);
        Assert.Equal(Pegi.Pegi16, result.Pegi);
        Assert.Equal("https://example.com/game.jpg", result.ImageUrl);
    }

    [Fact]
    public void MapToListModel_WithoutImageUrl_ReturnsNullImageUrl()
    {
        // Arrange
        var entity = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Game Without Image",
            Genre = Genre.Strategy,
            Description = "Desc",
            Pegi = Pegi.Pegi12,
            Developer = new DeveloperEntity { Name = "Dev" }
        };

        // Act
        var result = _mapper.MapToListModel(entity);

        // Assert
        Assert.Null(result.ImageUrl);
    }
}

public class UserModelMapperTests
    {
        private readonly UserModelMapper _mapper = new();

        [Fact]
        public void MapToListModel_WithValidEntity_ReturnsCorrectModel()
        {
            // Arrange
            var entity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                Name = "Test",
                Surname = "User",
                PhoneNumber = "1234567890",
                Library = new LibraryEntity { Name = "Test Library" }
            };

            // Act
            var result = _mapper.MapToListModel(entity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity.Id, result.Id);
            Assert.Equal("testuser", result.Username);
            Assert.Equal("test@example.com", result.Email);
        }

        [Fact]
        public void MapToDetailModel_WithValidEntity_ReturnsCorrectModel()
        {
            // Arrange
            var entity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Username = "johndoe",
                Email = "john@example.com",
                Name = "John",
                Surname = "Doe",
                PhoneNumber = "9876543210",
                Library = new LibraryEntity { Name = "John's Library" }
            };

            // Act
            var result = _mapper.MapToDetailModel(entity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("johndoe", result.Username);
            Assert.Equal("john@example.com", result.Email);
            Assert.Equal("John", result.Name);
            Assert.Equal("Doe", result.Surname);
        }

    [Fact]
    public void MapToEntity_WithValidModel_ReturnsCorrectEntity()
    {
        // Arrange
        var model = new UserDetailModel
        {
            Id = Guid.NewGuid(),
            Username = "newuser",
            Email = "new@example.com",
            Name = "New",
            Surname = "User",
            PhoneNumber = "9876543210",
            LibraryId = Guid.NewGuid()
        };

        // Act
        var result = _mapper.MapToEntity(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.Id, result.Id);
            Assert.Equal("newuser", result.Username);
            Assert.Equal("new@example.com", result.Email);
        }
    }

    public class DeveloperModelMapperTests
{
    private readonly DeveloperModelMapper _mapper = new();

    [Fact]
    public void MapToListModel_WithValidEntity_ReturnsCorrectModel()
    {
        // Arrange
        var entity = new DeveloperEntity
        {
            Id = Guid.NewGuid(),
            Name = "Rockstar Games"
        };

        // Act
        var result = _mapper.MapToListModel(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
        Assert.Equal("Rockstar Games", result.Name);
    }

    [Fact]
    public void MapToDetailModel_WithValidEntity_ReturnsCorrectModel()
    {
        // Arrange
        var entity = new DeveloperEntity
        {
            Id = Guid.NewGuid(),
            Name = "CD Projekt Red"
        };

        // Act
        var result = _mapper.MapToDetailModel(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("CD Projekt Red", result.Name);
    }

    [Fact]
    public void MapToEntity_WithValidModel_ReturnsCorrectEntity()
    {
        // Arrange
        var model = new DeveloperDetailModel
        {
            Id = Guid.NewGuid(),
            Name = "New Studio"
        };

        // Act
        var result = _mapper.MapToEntity(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.Id, result.Id);
        Assert.Equal("New Studio", result.Name);
    }
}

public class LibraryModelMapperTests
{
    private readonly LibraryModelMapper _mapper = new();

    [Fact]
    public void MapToListModel_WithValidEntity_ReturnsCorrectModel()
    {
        // Arrange
        var entity = new LibraryEntity
        {
            Id = Guid.NewGuid(),
            Name = "My Collection"
        };

        // Act
        var result = _mapper.MapToListModel(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
        Assert.Equal("My Collection", result.Name);
    }

    [Fact]
    public void MapToDetailModel_WithValidEntity_ReturnsCorrectModel()
    {
        // Arrange
        var entity = new LibraryEntity
        {
            Id = Guid.NewGuid(),
            Name = "Game Library"
        };

        // Act
        var result = _mapper.MapToDetailModel(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Game Library", result.Name);
    }

    [Fact]
    public void MapToEntity_WithValidModel_ReturnsCorrectEntity()
    {
        // Arrange
        var model = new LibraryDetailModel
        {
            Id = Guid.NewGuid(),
            Name = "New Library"
        };

        // Act
        var result = _mapper.MapToEntity(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Library", result.Name);
    }
}
