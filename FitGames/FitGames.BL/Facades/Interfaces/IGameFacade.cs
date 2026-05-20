using FitGames.BL.Models;
using FitGames.DAL.Entities;
using FitGames.DAL.Enums;

namespace FitGames.BL.Facades.Interfaces;

public interface IGameFacade : IFacade<GameEntity, GameListModel, GameDetailModel>
{
    Task<IEnumerable<GameListModel>> FilterGamesAsync(string? name, Genre? genre, Pegi? pegi, bool ascending = true, Guid? libraryId = null);
}