using FitGames.DAL.Entities;
using FitGames.BL.Models;

namespace FitGames.BL.Mappers;

public class LibraryModelMapper : ModelMapperBase<LibraryEntity, LibraryListModel, LibraryDetailModel>
{
    private readonly GameModelMapper _gameMapper = new();

    public override LibraryListModel MapToListModel(LibraryEntity? entity)
        => entity is null
            ? LibraryListModel.Empty
            : new LibraryListModel
            {
                Id = entity.Id,
                Name = entity.Name
            };

    public override LibraryDetailModel MapToDetailModel(LibraryEntity? entity)
        => entity is null
            ? LibraryDetailModel.Empty
            : new LibraryDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Games = new System.Collections.ObjectModel.ObservableCollection<GameListModel>(
                    entity.Games.Select(g => _gameMapper.MapToListModel(g)).ToList()
                )
            };

    public override LibraryEntity MapToEntity(LibraryDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name
        };
}
