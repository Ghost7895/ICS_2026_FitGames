using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Mappers;
using FitGames.BL.Models;
using FitGames.DAL.Entities;
using FitGames.DAL.Enums;
using FitGames.DAL.Mappers;
using FitGames.DAL.UnitOfWork;

namespace FitGames.BL.Facades;

public class GameFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    GameModelMapper modelMapper)
    : FacadeBase<GameEntity, GameListModel, GameDetailModel, GameEntityMapper>(unitOfWorkFactory, modelMapper),
        IGameFacade
{
    protected override ICollection<string> IncludesNavigationPathDetail =>
        new[] { nameof(GameEntity.Developer), nameof(GameEntity.Libraries) };

    public async Task<IEnumerable<GameListModel>> FilterGamesAsync(string? name, Genre? genre, Pegi? pegi, bool ascending = true)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        var searchName = name?.ToLower();

        var entities = await uow.GetRepository<GameEntity, GameEntityMapper>()
            .GetAllAsync(filter: g => 
                (string.IsNullOrWhiteSpace(searchName) || g.Name.ToLower().Contains(searchName)) &&
                (!genre.HasValue || genre.Value == Genre.Unknown || g.Genre == genre.Value) &&
                (!pegi.HasValue || pegi.Value == Pegi.Unknown || g.Pegi == pegi.Value));

        return ascending
            ? ModelMapper.MapToListModel(entities.OrderBy(g => g.Name))
            : ModelMapper.MapToListModel(entities.OrderByDescending(g => g.Name));
    }
}