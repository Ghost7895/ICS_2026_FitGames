using FitGames.DAL.Entities;

namespace FitGames.DAL.Mappers;

public class UserEntityMapper : IEntityMapper<UserEntity>
{
    public void MapToExistingEntity(UserEntity existingEntity, UserEntity newEntity)
    {
        existingEntity.Username = newEntity.Username;
        existingEntity.Email = newEntity.Email;
        existingEntity.Name = newEntity.Name;
        existingEntity.Surname = newEntity.Surname;
        existingEntity.PhoneNumber = newEntity.PhoneNumber;
    }
}
