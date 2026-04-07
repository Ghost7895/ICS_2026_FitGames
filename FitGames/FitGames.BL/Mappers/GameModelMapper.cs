using FitGames.DAL.Entities;
using FitGames.BL.Models;

namespace FitGames.BL.Mappers;

public class GameModelMapper : ModelMapperBase<GameEntity, GameListModel, GameDetailModel>
{
    public override GameListModel MapToListModel(GameEntity? entity)
        => entity is null
            ? GameListModel.Empty
            : new GameListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                ImageUrl = entity.ImageUrl,
                Genre = entity.Genre
            };

    public override GameDetailModel MapToDetailModel(GameEntity? entity)
        => entity is null
            ? GameDetailModel.Empty
            : new GameDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Pegi = entity.Pegi,
                Genre = entity.Genre,
                ImageUrl = entity.ImageUrl,
                DeveloperName = entity.Developer?.Name ?? "Unknown"
            };

    public override GameEntity MapToEntity(GameDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Pegi = model.Pegi,
            Genre = model.Genre,
            ImageUrl = model.ImageUrl,
            Developer = null!
        };
}
