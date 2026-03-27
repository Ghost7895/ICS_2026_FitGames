using FitGames.DAL.Entities;

namespace FitGames.DAL.Mappers
{
    public interface IEntityMapper<in TEntity>
        where TEntity : IEntity
    {
        void MapToExistingEntity(TEntity existingEntity, TEntity newEntity);
    }
}
