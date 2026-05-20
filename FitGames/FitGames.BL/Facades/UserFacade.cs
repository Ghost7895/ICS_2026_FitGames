using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Mappers;
using FitGames.BL.Models;
using FitGames.DAL.Entities;
using FitGames.DAL.Mappers;
using FitGames.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

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
                                .GetAllAsync(
                                    filter: u => u.Username == username,
                                    includePaths: new[] { nameof(UserEntity.Library) });

        return ModelMapper.MapToDetailModel(entities.SingleOrDefault());
    }
}
