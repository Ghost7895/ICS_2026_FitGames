using FitGames.DAL.Entities;

namespace FitGames.DAL.Mappers;

public class GameEntityMapper : IEntityMapper<GameEntity>
{
    public void MapToExistingEntity(GameEntity existingEntity, GameEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.Description = newEntity.Description;
        existingEntity.Pegi = newEntity.Pegi;
        existingEntity.Genre = newEntity.Genre;
        existingEntity.ImageUrl = newEntity.ImageUrl;
        existingEntity.DeveloperId = newEntity.DeveloperId;
    }
}