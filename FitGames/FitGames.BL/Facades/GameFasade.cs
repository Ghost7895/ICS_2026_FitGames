using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Mappers;
using FitGames.BL.Models;
using FitGames.DAL.Entities;
using FitGames.DAL.Enums;
using FitGames.DAL.Mappers;
using FitGames.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

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
        var query = uow.GetRepository<GameEntity, GameEntityMapper>()
                       .Get()
                       .Where(g => g.Name.Contains(name))
                       .OrderBy(g => g.Name);

        var entities = await query.ToListAsync();
        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<GameListModel>> FilterGamesByGenreAsync(Genre genre)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var query = uow.GetRepository<GameEntity, GameEntityMapper>()
                       .Get()
                       .Where(g => g.Genre == genre);

        var entities = await query.ToListAsync();
        return ModelMapper.MapToListModel(entities);
    }
}
