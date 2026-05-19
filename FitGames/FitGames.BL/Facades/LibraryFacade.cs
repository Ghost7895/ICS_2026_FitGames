using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Mappers;
using FitGames.BL.Models;
using FitGames.DAL.Entities;
using FitGames.DAL.Mappers;
using FitGames.DAL.UnitOfWork;

namespace FitGames.BL.Facades;

public class LibraryFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    LibraryModelMapper modelMapper)
    : FacadeBase<LibraryEntity, LibraryListModel, LibraryDetailModel, LibraryEntityMapper>(unitOfWorkFactory, modelMapper), 
        ILibraryFacade
{
    protected override ICollection<string> IncludesNavigationPathDetail =>
        new[] { nameof(LibraryEntity.Games) };

    public async Task AddGameToLibraryAsync(Guid libraryId, Guid gameId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        var libraryRepo = uow.GetRepository<LibraryEntity, LibraryEntityMapper>();
        var library = await libraryRepo.GetByIdAsync(libraryId, new[] { nameof(LibraryEntity.Games) }, true);

        if (library == null) throw new InvalidOperationException("Library not found.");

        var gameRepo = uow.GetRepository<GameEntity, GameEntityMapper>();
        var game = await gameRepo.GetByIdAsync(gameId, null, true);

        if (game == null) throw new InvalidOperationException("Game not found.");

        if (!library.Games.Any(g => g.Id == gameId))
        {
            library.Games.Add(game);
        }

        await libraryRepo.UpdateAsync(library);
        await uow.CommitAsync();
    }

    public async Task RemoveGameFromLibraryAsync(Guid libraryId, Guid gameId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        var libraryRepo = uow.GetRepository<LibraryEntity, LibraryEntityMapper>();
        var library = await libraryRepo.GetByIdAsync(libraryId, new[] { nameof(LibraryEntity.Games) }, true);

        if (library == null) throw new InvalidOperationException("Library not found.");

        var gameToRemove = library.Games.SingleOrDefault(g => g.Id == gameId);

        if (gameToRemove != null)
        {
            library.Games.Remove(gameToRemove);
            await libraryRepo.UpdateAsync(library);
            await uow.CommitAsync();
        }
    }

    public async Task CreateLibraryForUserAsync(Guid userId, string username)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var libraryRepo = uow.GetRepository<LibraryEntity, LibraryEntityMapper>();

        await libraryRepo.InsertAsync(new LibraryEntity
        {
            Id = Guid.NewGuid(),
            Name = $"{username}'s Library",
            UserId = userId
        });

        await uow.CommitAsync();
    }
}
