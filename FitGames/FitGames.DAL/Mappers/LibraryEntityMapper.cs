using FitGames.DAL.Entities;

namespace FitGames.DAL.Mappers;

public class LibraryEntityMapper : IEntityMapper<LibraryEntity>
{
    public void MapToExistingEntity(LibraryEntity existingEntity, LibraryEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
    }
}
