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

    public async Task<IEnumerable<GameListModel>> FilterGamesAsync(string? name, Genre? genre)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        var searchName = name?.ToLower();

        var entities = await uow.GetRepository<GameEntity, GameEntityMapper>()
            .GetAllAsync(filter: g => 
                (string.IsNullOrWhiteSpace(searchName) || g.Name.ToLower().Contains(searchName)) &&
                (!genre.HasValue || genre.Value == Genre.Unknown || g.Genre == genre.Value));

        return ModelMapper.MapToListModel(entities.OrderBy(g => g.Name));
    }
}
