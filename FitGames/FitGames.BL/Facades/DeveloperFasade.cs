using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Mappers;
using FitGames.BL.Models;
using FitGames.DAL.Entities;
using FitGames.DAL.Mappers;
using FitGames.DAL.UnitOfWork;

namespace FitGames.BL.Facades;

public class DeveloperFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    DeveloperModelMapper modelMapper)
    : FacadeBase<DeveloperEntity, DeveloperListModel, DeveloperDetailModel, DeveloperEntityMapper>(unitOfWorkFactory, modelMapper), 
        IDeveloperFacade
{
    protected override ICollection<string> IncludesNavigationPathDetail =>
        new[] { nameof(DeveloperEntity.PublishedGames) };
}
