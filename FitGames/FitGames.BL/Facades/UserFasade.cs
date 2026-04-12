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
        var query = uow.GetRepository<UserEntity, UserEntityMapper>()
                       .Get()
                       .Include(u => u.Library)
                       .SingleOrDefaultAsync(u => u.Username == username);

        var entity = await query;
        return ModelMapper.MapToDetailModel(entity);
    }
}
