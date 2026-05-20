using FitGames.BL.Models;

namespace FitGames.app.Services.Interfaces;

public interface IUserSessionService
{
    UserDetailModel? CurrentUser { get; set; }
}