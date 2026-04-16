using FitGames.BL.Models;
using FitGames.DAL.Entities;

namespace FitGames.BL.Facades.Interfaces;

public interface IUserFacade : IFacade<UserEntity, UserListModel, UserDetailModel>
{
    Task<UserDetailModel?> GetUserByUsernameAsync(string username);
}
