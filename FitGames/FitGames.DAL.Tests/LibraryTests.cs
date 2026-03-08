using FitGames.DAL.Entities;
using FitGames.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Tests
{
    public class LibraryTests : DbContextTestsBase
    {
        private async Task<LibraryEntity> Create_And_Save_Library_Entity(string name = "Library")
        {
            LibraryEntity library = new() { Name = name };
            GameDbContextSut.Libraries.Add(library);
            await GameDbContextSut.SaveChangesAsync();
            return library;
        }

        [Fact]
        public async Task Add_New_Library_Entity()
        {
            // Arrange
            const string libraryName = "library";
            LibraryEntity library = new() { Name = libraryName };

            // Act
            GameDbContextSut.Libraries.Add(library);
            await GameDbContextSut.SaveChangesAsync();


            //Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Libraries.First(lib => lib.Id == library.Id);
            Assert.Equal(library.Id, entityFromDb.Id);
            Assert.Equal(libraryName, entityFromDb.Name);
        }

        [Fact]
        public async Task Add_New_Library_With_Game()
        {
            // Arrange
            var library = await Create_And_Save_Library_Entity();
            GameEntity game = new()
            {
                Name = "Minecraft",
                Description = "Block game",
                Genre = Genre.Sandbox,
                Pegi = Pegi.Pegi3,
                Developer = new DeveloperEntity()
                {
                    Name = "Mojang"
                }
            };


            // Act
            library.Games.Add(game);
            await GameDbContextSut.SaveChangesAsync();

            //Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Libraries.Include(l => l.Games).First(lib => lib.Id == library.Id);
            Assert.Equal(game.Id, entityFromDb.Games.Single().Id);
        }

        [Fact]
        public async Task Add_New_Library_With_Multiple_Games()
        {
            // Arrange
            const int amountOfGames = 2;
            var library = await Create_And_Save_Library_Entity();
            GameEntity game1 = new()
            {
                Name = "Minecraft",
                Description = "Block game",
                Genre = Genre.Sandbox,
                Pegi = Pegi.Pegi3,
                Developer = new DeveloperEntity()
                {
                    Name = "Mojang"
                }
            };
            GameEntity game2 = new()
            {
                Name = "Minecraft2",
                Description = "Block game2",
                Developer = new DeveloperEntity() { Name = "ahoj" },
                Genre = Genre.Sandbox,
                Pegi = Pegi.Pegi3,
            };
            
            // Act
            library.Games.Add(game1);
            library.Games.Add(game2);
            await GameDbContextSut.SaveChangesAsync();


            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Libraries.Include(l => l.Games).First(lib => lib.Id == library.Id);
            Assert.Equal(amountOfGames, entityFromDb.Games.Count);
            Assert.Contains(entityFromDb.Games, g => g.Id == game1.Id);
            Assert.Contains(entityFromDb.Games, g => g.Id == game2.Id);
        }

        [Fact]
        public async Task UpdateLibraryName()
        {
            // Arrange
            const string newName = "Nove meno";
            var library = await Create_And_Save_Library_Entity();

            // Act
            library.Name = newName;
            await GameDbContextSut.SaveChangesAsync();

            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Libraries.First(lib => lib.Id == library.Id);
            Assert.Equal(newName, entityFromDb.Name);
        }

        [Fact]
        public async Task Remove_Game_From_Library()
        {
            // Arrange
            var library = await Create_And_Save_Library_Entity();
            GameEntity game = new()
            {
                Name = "Minecraft2",
                Description = "Block game2",
                Developer = new DeveloperEntity() { Name = "ahoj" },
                Genre = Genre.Sandbox,
                Pegi = Pegi.Pegi3,
            };
            library.Games.Add(game);
            await GameDbContextSut.SaveChangesAsync();
            Assert.Equal(game.Id, library.Games.Single().Id);


            //Act
            library.Games.Remove(game);
            await GameDbContextSut.SaveChangesAsync();

            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Libraries.Include(l => l.Games).First(lib => lib.Id == library.Id);
            Assert.DoesNotContain(entityFromDb.Games, g => g.Id == game.Id);
        }

        [Fact]
        public async Task Remove_Library_From_DB()
        {
            // Arrange
            var library = await Create_And_Save_Library_Entity();

            // Act
            GameDbContextSut.Libraries.Remove(library);
            await GameDbContextSut.SaveChangesAsync();

            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            Assert.DoesNotContain(dbx.Libraries, l => l.Id == library.Id);
        }

        [Fact]
        public async Task Remove_Library_With_Game()
        {
            // Arrange
            var library = await Create_And_Save_Library_Entity();
            GameEntity game = new()
            {
                Name = "Minecraft2",
                Description = "Block game2",
                Developer = new DeveloperEntity() { Name = "ahoj" },
                Genre = Genre.Sandbox,
                Pegi = Pegi.Pegi3,
            };

            library.Games.Add(game);
            await GameDbContextSut.SaveChangesAsync();
            Assert.Contains(game.Libraries, l => l.Id == library.Id);

            // Act
            GameDbContextSut.Libraries.Remove(library);
            await GameDbContextSut.SaveChangesAsync();

            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Games.Include(g => g.Libraries).First(g => g.Id == game.Id);
            Assert.DoesNotContain(entityFromDb.Libraries, l => l.Id == library.Id);
        }


    }
}
