using FitGames.DAL.Entities;

namespace FitGames.DAL.Mappers;

public class DeveloperEntityMapper : IEntityMapper<DeveloperEntity>
{
    public void MapToExistingEntity(DeveloperEntity existingEntity, DeveloperEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
    }
}
