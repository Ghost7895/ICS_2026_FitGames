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

    public async Task<IEnumerable<GameListModel>> FilterGamesAsync(string? name, Genre? genre, Pegi? pegi, bool ascending = true, Guid? libraryId = null)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        var searchName = name?.ToLower();

        var entities = await uow.GetRepository<GameEntity, GameEntityMapper>()
            .GetAllAsync(
            filter: g => 
                (string.IsNullOrWhiteSpace(searchName) || g.Name.ToLower().Contains(searchName)) &&
                (!genre.HasValue || genre.Value == Genre.Unknown || g.Genre == genre.Value) &&
                (!pegi.HasValue || pegi.Value == Pegi.Unknown || g.Pegi == pegi.Value) &&
                (!libraryId.HasValue || g.Libraries.Any(l => l.Id == libraryId.Value)),
            orderBy: g => g.Name,
            orderAscending: ascending);

        return ModelMapper.MapToListModel(entities);
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

        GameEntity? gameEntity = null;
        bool newGameDetected = model.Id == Guid.Empty;

        if (!newGameDetected)
        {
            gameEntity = await gameRepository.GetByIdAsync(model.Id).ConfigureAwait(false);
        }

        if (gameEntity == null)
        {
            newGameDetected = true;
            gameEntity = new GameEntity()
            {
                Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id,
                Name = string.Empty,
                Description = string.Empty,
                Pegi = Pegi.Unknown,
                Genre = Genre.Unknown
            };
        }

        gameEntity.Name = model.Name;
        gameEntity.Description = model.Description;
        gameEntity.ImageUrl = model.ImageUrl;
        gameEntity.Genre = model.Genre;
        gameEntity.Pegi = model.Pegi;

        string typedName = model.DeveloperName?.Trim() ?? "Unknown";

        var existingDevelopers = await developerRepository.GetAllAsync(
                filter: d => d.Name.ToLower() == typedName.ToLower(),
                take: 1
                ).ConfigureAwait(false);

        var matchedDev = existingDevelopers.FirstOrDefault();

        if (matchedDev != null)
        {
            gameEntity.DeveloperId = matchedDev.Id;
            gameEntity.Developer = null!;
        }
        else
        {
            matchedDev = new DeveloperEntity
            {
                Id = Guid.NewGuid(),
                Name = typedName
            };

            await developerRepository.InsertAsync(matchedDev).ConfigureAwait(false);

            gameEntity.DeveloperId = matchedDev.Id;
            gameEntity.Developer = null!;
        }

        if (newGameDetected)
        {
            await gameRepository.InsertAsync(gameEntity).ConfigureAwait(false);
        }
        else
        {
            await gameRepository.UpdateAsync(gameEntity).ConfigureAwait(false);
        }

        await uow.CommitAsync().ConfigureAwait(false);
        gameEntity.Developer = matchedDev;
        return ModelMapper.MapToDetailModel(gameEntity);
    }

    public override async Task DeleteAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        var gameRepo = uow.GetRepository<GameEntity, GameEntityMapper>();
        var gameToCheck = new GameEntity()
        {
            Id = id,
            Name = string.Empty,
            Pegi = Pegi.Unknown,
            Genre = Genre.Unknown
        };

        if (await gameRepo.ExistAsync(gameToCheck))
        {
            await gameRepo.DeleteAsync(id);
            await uow.CommitAsync();
        }
    }
}