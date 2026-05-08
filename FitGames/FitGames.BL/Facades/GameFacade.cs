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

    public async Task<IEnumerable<GameListModel>> FilterGamesByNameAsync(string name)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var entities = await uow.GetRepository<GameEntity, GameEntityMapper>()
                                .GetAllAsync(filter: g => g.Name.Contains(name));

        return ModelMapper.MapToListModel(entities.OrderBy(g => g.Name));
    }

    public async Task<IEnumerable<GameListModel>> FilterGamesByGenreAsync(Genre genre)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var entities = await uow.GetRepository<GameEntity, GameEntityMapper>()
                                .GetAllAsync(filter: g => g.Genre == genre);

        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<GameListModel>> FilterGamesByPegiAsync(Pegi pegi)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var entities = await uow.GetRepository<GameEntity, GameEntityMapper>()
                                .GetAllAsync(filter: g => g.Pegi == pegi);

        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<GameListModel>> GetGamesSortedByNameAsync(bool ascending = true)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var entities = await uow.GetRepository<GameEntity, GameEntityMapper>()
                                .GetAllAsync();

        return ascending
            ? ModelMapper.MapToListModel(entities.OrderBy(g => g.Name))
            : ModelMapper.MapToListModel(entities.OrderByDescending(g => g.Name));
    }
}