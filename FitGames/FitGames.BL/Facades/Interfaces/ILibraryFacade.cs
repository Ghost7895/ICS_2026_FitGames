using FitGames.BL.Models;
using FitGames.DAL.Entities;

namespace FitGames.BL.Facades.Interfaces;

public interface ILibraryFacade : IFacade<LibraryEntity, LibraryListModel, LibraryDetailModel>
{
    Task AddGameToLibraryAsync(Guid libraryId, Guid gameId);
    Task RemoveGameFromLibraryAsync(Guid libraryId, Guid gameId);
    Task CreateLibraryForUserAsync(Guid userId, string username);
}
