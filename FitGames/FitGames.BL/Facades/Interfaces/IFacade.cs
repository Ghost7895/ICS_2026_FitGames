namespace FitGames.BL.Facades.Interfaces;

public interface IFacade<TEntity, TListModel, TDetailModel>
{
    Task DeleteAsync(Guid id);
    Task<TDetailModel?> GetAsync(Guid id);
    Task<IEnumerable<TListModel>> GetAsync();
    Task<TDetailModel> SaveAsync(TDetailModel model);
}