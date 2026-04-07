using System.Collections;
using System.Reflection;
using FitGames.BL.Mappers;
using FitGames.BL.Models;
using FitGames.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitGames.BL.Facades;

public abstract class FacadeBase<TEntity, TListModel, TDetailModel>
    : IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : ModelBase
    where TDetailModel : ModelBase
{
    protected readonly IModelMapper<TEntity, TListModel, TDetailModel> ModelMapper;

    protected FacadeBase(IModelMapper<TEntity, TListModel, TDetailModel> modelMapper)
    {
        ModelMapper = modelMapper;
    }

    protected virtual ICollection<string> IncludesNavigationPathDetail => new List<string>();

    public virtual Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public virtual Task<TDetailModel?> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public virtual Task<IEnumerable<TListModel>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public virtual Task<TDetailModel> SaveAsync(TDetailModel model)
    {
        throw new NotImplementedException();
    }

    private static void GuardCollectionsAreNotSet(TDetailModel model)
    {
        IEnumerable<PropertyInfo> collectionProperties = model
            .GetType()
            .GetProperties()
            .Where(i => typeof(ICollection).IsAssignableFrom(i.PropertyType));

        foreach (PropertyInfo collectionProperty in collectionProperties)
        {
            if (collectionProperty.GetValue(model) is ICollection { Count: > 0 })
            {
                throw new InvalidOperationException(
                    "Current BL and DAL infrastructure disallows insert or update of models with adjacent collections.");
            }
        }
    }
}
