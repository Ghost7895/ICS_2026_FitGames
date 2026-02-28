using FitGames.DAL.Entities;
using FitGames.DAL.Enums;
using FitGames.DAL.Factories;

namespace FitGames.DAL.Tests
{
    public class GameTests : DbContextTestsBase
    {
        [Fact]
        public async Task AddNew_Game_Persisted()
        {
            //  Arrange
            GameEntity entity = new()
            {
                Name = "Minecraft",
                Description = "Block game",
                Genre = Genre.Sandbox,
                Pegi = Pegi.Pegi3
            };

            //  Act
            GameDbContextSut.Games.Add(entity);
            await GameDbContextSut.SaveChangesAsync();

            //  Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Games.First(GameEntity => GameEntity.Id == entity.Id);
            Assert.Equal(entity, entityFromDb);
        }
    }
}
