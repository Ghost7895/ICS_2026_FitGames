using FitGames.BL.Mappers;
using FitGames.BL.Models;
using FitGames.DAL.Entities;
using FitGames.DAL.Mappers;
using FitGames.DAL.UnitOfWork;

namespace FitGames.BL.Facades;

public class GameFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    GameModelMapper modelMapper)
    : FacadeBase<GameEntity, GameListModel, GameDetailModel, GameEntityMapper>(unitOfWorkFactory, modelMapper)
{
    protected override ICollection<string> IncludesNavigationPathDetail => new List<string>
    {
        nameof(GameEntity.Developer)
    };
}