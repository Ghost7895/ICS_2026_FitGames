
using FitGames.DAL.Entities;

namespace FitGames.app.Services.Interfaces
{
    public interface IUserContext
    {
        Guid? CurrentUserId { get; set; }
        Guid? CurrentLibraryId { get; set; }
        string? Username { get; set; }

        bool IsLoggedIn => CurrentUserId.HasValue;

        void Initialize(UserEntity user);

    }
}
