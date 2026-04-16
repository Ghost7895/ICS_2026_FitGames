using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Mappers;
using FitGames.BL.Models;
using FitGames.DAL.Entities;
using FitGames.DAL.Mappers;
using FitGames.DAL.UnitOfWork;

namespace FitGames.BL.Facades;

public class UserFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    UserModelMapper modelMapper)
    : FacadeBase<UserEntity, UserListModel, UserDetailModel, UserEntityMapper>(unitOfWorkFactory, modelMapper),
        IUserFacade
{
    protected override ICollection<string> IncludesNavigationPathDetail =>
        new[] { nameof(UserEntity.Library) };

    public async Task<UserDetailModel?> GetUserByUsernameAsync(string username)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var entities = await uow.GetRepository<UserEntity, UserEntityMapper>()
                                .GetAllAsync(new[] { nameof(UserEntity.Library) });

        var entity = entities.SingleOrDefault(u => u.Username == username);
        return ModelMapper.MapToDetailModel(entity);
    }
}
