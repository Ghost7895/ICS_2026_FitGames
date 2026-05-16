using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Mappers;
using FitGames.BL.Models;
using FitGames.DAL.Entities;
using FitGames.DAL.Enums;
using FitGames.DAL.Mappers;
using FitGames.DAL.Repositories;
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

    public override async Task<GameDetailModel> SaveAsync(GameDetailModel model)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model), "Model cannot be null");

        if (string.IsNullOrWhiteSpace(model.Name))
            throw new InvalidOperationException("Name is required and cannot be empty or whitespace");

        GuardCollectionsAreNotSet(model);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        var developerRepository = uow.GetRepository<DeveloperEntity, DeveloperEntityMapper>();
        var gameRepository = uow.GetRepository<GameEntity, GameEntityMapper>();

        var foundGameEntity = await gameRepository.GetByIdAsync(model.Id).ConfigureAwait(false);

        if (foundGameEntity == null)
        {
            throw new InvalidOperationException($"Game with ID {model.Id} not found.");
        }

        foundGameEntity.Name = model.Name;
        foundGameEntity.Description = model.Description;
        foundGameEntity.ImageUrl = model.ImageUrl;
        foundGameEntity.Genre = model.Genre;
        foundGameEntity.Pegi = model.Pegi;

        string typedName = model.DeveloperName?.Trim() ?? "Unknown";

        var existingDevelopers = await developerRepository.GetAllAsync(
                filter: d => d.Name.ToLower() == typedName.ToLower(),
                take: 1
                ).ConfigureAwait(false);

        var matchedDev = existingDevelopers.FirstOrDefault();

        if (matchedDev != null)
        {
            foundGameEntity.DeveloperId = matchedDev.Id;
            foundGameEntity.Developer = matchedDev;
        }
        else
        {
            var newDev = new DeveloperEntity
            {
                Id = Guid.NewGuid(),
                Name = typedName
            };

            await developerRepository.InsertAsync(newDev).ConfigureAwait(false);

            foundGameEntity.DeveloperId = newDev.Id;
            foundGameEntity.Developer = newDev;
        }

        await gameRepository.UpdateAsync(foundGameEntity).ConfigureAwait(false);

        await uow.CommitAsync().ConfigureAwait(false);

        return ModelMapper.MapToDetailModel(foundGameEntity);
    }
}