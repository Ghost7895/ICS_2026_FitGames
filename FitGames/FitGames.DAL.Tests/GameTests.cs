using FitGames.DAL.Entities;
using FitGames.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Tests
{
    public class GameTests : DbContextTestsBase
    {
        private async Task<GameEntity> Create_And_Save_Game_Entity(string name = "Minecraft",
            string description = "Block game", Genre genre = Genre.Sandbox, Pegi pegi = Pegi.Pegi3,
            string devName = "Mojang", string? imgUrl = null)
        {
            GameEntity entity = new()
            {
                Name = name,
                Description = description,
                Genre = genre,
                Pegi = pegi,
                Developer = new DeveloperEntity()
                {
                    Name = devName
                },
                ImageUrl = imgUrl,
            };

            GameDbContextSut.Games.Add(entity);
            await GameDbContextSut.SaveChangesAsync();

            return entity;
        }
        [Fact]
        public async Task AddNew_Game_Persisted()
        {
            //  Arrange

            const string name = "Minecraft";
            const string description = "Block game";
            const Genre genre = Genre.Sandbox;
            const Pegi pegi = Pegi.Pegi3;
            const string developerName = "Mojang";

            GameEntity entity = new()
            {
                Name = name,
                Description = description,
                Genre = genre,
                Pegi = pegi,
                Developer = new DeveloperEntity()
                { 
                    Name = developerName 
                }
            };

            //  Act
            GameDbContextSut.Games.Add(entity);
            await GameDbContextSut.SaveChangesAsync();


            //  Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Games.Include(g => g.Developer).First(GameEntity => GameEntity.Id == entity.Id);
            Assert.Equal(entity.Id, entityFromDb.Id);
            Assert.Equal(name, entityFromDb.Name);
            Assert.Equal(description, entityFromDb.Description);
            Assert.Equal(genre, entityFromDb.Genre);
            Assert.Equal(pegi, entityFromDb.Pegi);
            Assert.Equal(entity.Developer.Id, entityFromDb.Developer.Id);
            Assert.Equal(developerName, entityFromDb.Developer.Name);
        }
        [Fact]
        public async Task AddNew_Game_with_ImageUrl_Persisted()
        {
            //  Arrange
            const string url = "www.Url.sk";
            GameEntity entity = new()
            {
                Name = "Minecraft",
                Description = "Block game",
                Genre = Genre.Sandbox,
                Pegi = Pegi.Pegi3,
                Developer = new DeveloperEntity()
                {
                    Name = "Mojang"
                },
                ImageUrl = url,
            };

            //  Act
            GameDbContextSut.Games.Add(entity);
            await GameDbContextSut.SaveChangesAsync();

            //  Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Games.First(gameEntity => gameEntity.Id == entity.Id);
            Assert.Equal(url, entityFromDb.ImageUrl);
        }

        [Fact]
        public async Task AddNew_Game_With_Library()
        {
            // Arrange
            const string libraryName = "Ahoj";
            LibraryEntity library = new() { Name = libraryName };
            var entity = await Create_And_Save_Game_Entity();
            

            // Act
            entity.Libraries.Add(library);
            await GameDbContextSut.SaveChangesAsync();

            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Games.Include(g => g.Libraries).First(gameEntity => gameEntity.Id == entity.Id);
            Assert.Equal(library.Id, entityFromDb.Libraries.Single().Id);
        }

        [Fact]
        public async Task AddNew_Game_With_Multiple_Libraries()
        {
            // Arrange
            const int amountOfLibraries = 2;
            LibraryEntity library1 = new() { Name = "libraryName1" };
            LibraryEntity library2 = new() { Name = "libraryName2" };
            var entity = await Create_And_Save_Game_Entity();

            // Act
            entity.Libraries.Add(library1);
            entity.Libraries.Add(library2);
            await GameDbContextSut.SaveChangesAsync();

            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Games.Include(g => g.Libraries).First(gameEntity => gameEntity.Id == entity.Id);
            Assert.Equal(amountOfLibraries, entityFromDb.Libraries.Count);
            Assert.Contains(entityFromDb.Libraries, l => l.Id == library1.Id);
            Assert.Contains(entityFromDb.Libraries, l => l.Id == library2.Id);
        }
        [Fact]
        public async Task Update_Game_Name()
        {
            //Arrange
            var entity = await Create_And_Save_Game_Entity();
            const string newName = "Roblox"; 
            //  Act
            entity.Name = newName;
            await GameDbContextSut.SaveChangesAsync();

            //  Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Games.First(g => g.Id == entity.Id);
            Assert.Equal(newName, entityFromDb.Name);
        }

        [Fact]
        public async Task Update_Game_Description()
        {
            // Arrange
            var entity = await Create_And_Save_Game_Entity();
            const string newDescription = "Great game to kill boredom";

            // Act
            entity.Description = newDescription;
            await GameDbContextSut.SaveChangesAsync();

            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Games.First(g => g.Id == entity.Id);
            Assert.Equal(newDescription, entityFromDb.Description);
        }

        [Fact]
        public async Task Update_Game_Genre()
        {
            // Arrange
            const Genre newGenre = Genre.Action;
            var entity = await Create_And_Save_Game_Entity(genre: Genre.Sandbox);

            // Act
            entity.Genre = newGenre;
            await GameDbContextSut.SaveChangesAsync();

            //Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Games.First(g => g.Id == entity.Id);
            Assert.Equal(newGenre, entityFromDb.Genre);
        }

        [Fact]
        public async Task Update_Game_Pegi()
        {
            // Arrange
            const Pegi newPegi= Pegi.Pegi16;
            var entity = await Create_And_Save_Game_Entity(pegi: Pegi.Pegi3);

            // Act
            entity.Pegi = newPegi;
            await GameDbContextSut.SaveChangesAsync();

            //Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Games.First(g => g.Id == entity.Id);
            Assert.Equal(newPegi, entityFromDb.Pegi);
        }

        [Fact]
        public async Task Update_Game_ImageUrl()
        {
            // Arrange
            const string oldUrl = "www.ahoj.sk";
            const string newUrl = "www.soj.cz";
            var entity = await Create_And_Save_Game_Entity(imgUrl: oldUrl);

            // Act
            entity.ImageUrl = newUrl;
            await GameDbContextSut.SaveChangesAsync();

            //Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Games.First(g => g.Id == entity.Id);
            Assert.Equal(newUrl, entityFromDb.ImageUrl);
        }

        [Fact]
        public async Task Update_Game_Url_NULL()
        {
            // Arrange
            var entity = await Create_And_Save_Game_Entity(imgUrl: "www.vsechnomijdenejlip.cz");

            // Act
            entity.ImageUrl = null;
            await GameDbContextSut.SaveChangesAsync();

            //Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Games.First(g => g.Id == entity.Id);
            Assert.Null(entityFromDb.ImageUrl);
        }

        [Fact]
        public async Task Remove_Game_From_DB()
        {
            // Arrange
            var entity = await Create_And_Save_Game_Entity();
            var dev = entity.Developer;
            Assert.Contains(dev.PublishedGames, g => g.Id == entity.Id);

            // Act
            GameDbContextSut.Games.Remove(entity);
            await GameDbContextSut.SaveChangesAsync();

            //assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            Assert.False(dbx.Games.Any(g => g.Id == entity.Id));
            Assert.DoesNotContain(dev.PublishedGames, g => g.Id == entity.Id);
        }

        [Fact]
        public async Task Remove_Library_From_Game()
        {
            // Arrange
            var entity = await Create_And_Save_Game_Entity();
            LibraryEntity library = new() { Name = "library" };
            entity.Libraries.Add(library);
            await GameDbContextSut.SaveChangesAsync();

            // Act
            entity.Libraries.Remove(library);
            await GameDbContextSut.SaveChangesAsync();

            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Games.Include(g => g.Libraries).First(g => g.Id == entity.Id);
            Assert.DoesNotContain(entityFromDb.Libraries, l => l.Id == library.Id);

        }

        [Fact]
        public async Task Remove_Game_In_Libraries()
        {
            // Arrange
            var entity = await Create_And_Save_Game_Entity();
            LibraryEntity library1 = new() { Name = "lib1" };
            LibraryEntity library2 = new() { Name = "lib2" };
            entity.Libraries.Add(library1);
            entity.Libraries.Add(library2);
            await GameDbContextSut.SaveChangesAsync();
            Assert.Contains(library1.Games, l => l.Id == entity.Id);
            Assert.Contains(library2.Games, l => l.Id == entity.Id);


            // Act
            GameDbContextSut.Games.Remove(entity);
            await GameDbContextSut.SaveChangesAsync();


            // Assert
            Assert.DoesNotContain(library1.Games, l => l.Id == entity.Id);
            Assert.DoesNotContain(library2.Games, l => l.Id == entity.Id);
        }
    }
}

