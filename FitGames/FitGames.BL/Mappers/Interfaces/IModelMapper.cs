namespace FitGames.BL.Mappers.Interfaces;

// This code was inspired by Cookbook Mapper Interface
public interface IModelMapper<TEntity, out TListModel, TDetailModel>
{
    TListModel MapToListModel(TEntity? entity);

    IEnumerable<TListModel> MapToListModel(IEnumerable<TEntity> entities)
        => entities.Select(MapToListModel);

    TDetailModel MapToDetailModel(TEntity? entity);

    TEntity MapToEntity(TDetailModel model);
}