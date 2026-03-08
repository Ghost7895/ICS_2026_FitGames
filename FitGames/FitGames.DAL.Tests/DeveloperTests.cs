using FitGames.DAL.Entities;
using FitGames.DAL.Enums;
using Microsoft.EntityFrameworkCore;


namespace FitGames.DAL.Tests
{
    public class DeveloperTests : DbContextTestsBase
    {
        private async Task<DeveloperEntity> Create_And_Save_Developer_Entity(string name = "Mojang")
        {
            DeveloperEntity developer = new(){ Name = name };
           
            GameDbContextSut.Developers.Add(developer);
            await GameDbContextSut.SaveChangesAsync();

            return developer;
        }

        [Fact]
        public async Task Add_New_Developer_Persisted() 
        {
            // Arrange
            const string name = "Mojang";
            DeveloperEntity developer = new() { Name = name };
            
            // Act
            GameDbContextSut.Developers.Add(developer);
            await GameDbContextSut.SaveChangesAsync();

            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Developers.First(Developer => Developer.Id == developer.Id);
            Assert.Equal(developer.Id, entityFromDb.Id);
            Assert.Equal(name, entityFromDb.Name);
        }

        [Fact]
        public async Task Add_New_Developer_With_Published_Game()
        {
            // Arrange
            var developer = await Create_And_Save_Developer_Entity();
            GameEntity game = new()
            {
                Name = "Minecraft",
                Description = "Block Game",
                Developer = developer,
                Genre = Genre.Sandbox,
                Pegi = Pegi.Pegi3,
            };
            

            // Act
            developer.PublishedGames.Add(game);
            await GameDbContextSut.SaveChangesAsync();


            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Developers.Include(g => g.PublishedGames).First(Developer => Developer.Id == developer.Id);
            Assert.Equal(game.Id, entityFromDb.PublishedGames.Single().Id);
        }

        [Fact]
        public async Task Add_New_Developer_With_Multiple_Published_Game()
        {
            // Arrange
            const int amountOfPublishedGames = 3;
            var developer = await Create_And_Save_Developer_Entity();
            GameEntity game1 = new()
            {
                Name = "Minecraft",
                Description = "Block game",
                Developer = developer,
                Genre = Genre.Sandbox,
                Pegi = Pegi.Pegi3,
            };
            GameEntity game2 = new()
            {
                Name = "Minecraft2",
                Description = "Block game2",
                Developer = developer,
                Genre = Genre.Sandbox,
                Pegi = Pegi.Pegi3,
            };
            GameEntity game3 = new()
            {
                Name = "Minecraft3",
                Description = "Block game3",
                Developer = developer,
                Genre = Genre.Sandbox,
                Pegi = Pegi.Pegi3,
            };


            // Act
            developer.PublishedGames.Add(game1);
            developer.PublishedGames.Add(game2);
            developer.PublishedGames.Add(game3);

            await GameDbContextSut.SaveChangesAsync();


            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Developers.Include(g => g.PublishedGames).First(Developer => Developer.Id == developer.Id);
            Assert.Equal(amountOfPublishedGames, entityFromDb.PublishedGames.Count);
            Assert.Contains(entityFromDb.PublishedGames, l => l.Id == game1.Id);
            Assert.Contains(entityFromDb.PublishedGames, l => l.Id == game2.Id);
            Assert.Contains(entityFromDb.PublishedGames, l => l.Id == game3.Id);
        }

        [Fact]
        public async Task Update_Developer_Name()
        {
            // Arrange
            const string newName = "Riot Games";
            var developer = await Create_And_Save_Developer_Entity();

            // Act
            developer.Name = newName;
            await GameDbContextSut.SaveChangesAsync();

            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Developers.First(Developer => Developer.Id == developer.Id);
            Assert.Equal(newName, entityFromDb.Name);
        }

        [Fact]
        public async Task Remove_Game_From_Published()
        {
            // Arrange
            var developer = await Create_And_Save_Developer_Entity();
            GameEntity game = new()
            {
                Name = "mine",
                Description = "nebavi",
                Developer = developer,
                Genre = Genre.Action,
                Pegi = Pegi.Pegi12
            };

            developer.PublishedGames.Add(game);
            await GameDbContextSut.SaveChangesAsync();

            // act 

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
            {
                developer.PublishedGames.Remove(game);
                return GameDbContextSut.SaveChangesAsync();
            }); 
        }

        [Fact]
        public async Task Remove_Developer_From_DB()
        {
            // Arrange
            var developer = await Create_And_Save_Developer_Entity();

            // act 
            GameDbContextSut.Developers.Remove(developer);
            await GameDbContextSut.SaveChangesAsync();

            //assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            Assert.False(dbx.Developers.Any(g => g.Id == developer.Id));
        }



        [Fact]
        public async Task Remove_Developer_With_Games_Error()
        {
            // Arrange
            var developer = await Create_And_Save_Developer_Entity();
            GameEntity game = new()
            {
                Name = "Minecraft",
                Description = "Block game",
                Genre = Genre.Sandbox,
                Pegi = Pegi.Pegi3,
                Developer = developer
            };

            developer.PublishedGames.Add(game);
            await GameDbContextSut.SaveChangesAsync();

            // Act and Assert

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
            {
                GameDbContextSut.Developers.Remove(developer);
                return GameDbContextSut.SaveChangesAsync();
            });
        }


    }
}
