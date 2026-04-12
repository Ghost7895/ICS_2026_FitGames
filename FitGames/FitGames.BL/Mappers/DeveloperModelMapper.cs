using FitGames.DAL.Entities;
using FitGames.BL.Models;

namespace FitGames.BL.Mappers;

public class DeveloperModelMapper : ModelMapperBase<DeveloperEntity, DeveloperListModel, DeveloperDetailModel>
{
    private readonly GameModelMapper _gameMapper = new();

    public override DeveloperListModel MapToListModel(DeveloperEntity? entity)
        => entity is null
            ? DeveloperListModel.Empty
            : new DeveloperListModel
            {
                Id = entity.Id,
                Name = entity.Name
            };

    public override DeveloperDetailModel MapToDetailModel(DeveloperEntity? entity)
        => entity is null
            ? DeveloperDetailModel.Empty
            : new DeveloperDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                PublishedGames = new System.Collections.ObjectModel.ObservableCollection<GameListModel>(
                    entity.PublishedGames.Select(g => _gameMapper.MapToListModel(g)).ToList()
                )
            };

    public override DeveloperEntity MapToEntity(DeveloperDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name
        };
}

