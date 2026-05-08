using FitGames.BL.Models;
using FitGames.DAL.Entities;
using FitGames.DAL.Enums;

namespace FitGames.BL.Facades.Interfaces;

public interface IGameFacade : IFacade<GameEntity, GameListModel, GameDetailModel>
{
    Task<IEnumerable<GameListModel>> FilterGamesByNameAsync(string name);
    Task<IEnumerable<GameListModel>> FilterGamesByGenreAsync(Genre genre);
    Task<IEnumerable<GameListModel>> FilterGamesByPegiAsync(Pegi pegi);
    Task<IEnumerable<GameListModel>> GetGamesSortedByNameAsync(bool ascending = true);
}