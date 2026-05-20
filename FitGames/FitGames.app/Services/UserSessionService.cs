using FitGames.app.Services.Interfaces;
using FitGames.BL.Models;

namespace FitGames.app.Services;

public class UserSessionService : IUserSessionService
{
    public UserDetailModel? CurrentUser { get; set; }
}