namespace FitGames.BL.Mappers;

public interface IModelMapper<TEntity, TListModel, TDetailModel>
{
    TListModel MapToListModel(TEntity? entity);
    IEnumerable<TListModel> MapToListModel(IEnumerable<TEntity> entities);
    TDetailModel MapToDetailModel(TEntity entity);
    TEntity MapToEntity(TDetailModel model);
}