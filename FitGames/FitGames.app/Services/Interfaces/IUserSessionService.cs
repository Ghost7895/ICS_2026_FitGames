using FitGames.BL.Models;

namespace FitGames.app.Services.Interfaces;

public interface IUserSessionService
{
    UserListModel? CurrentUser { get; set; }
}