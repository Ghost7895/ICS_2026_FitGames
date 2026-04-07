using FitGames.DAL.Entities;
using FitGames.DAL.Mappers;
using FitGames.DAL.Repositories;

namespace FitGames.DAL.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
        where TEntity : class, IEntity
        where TEntityMapper : IEntityMapper<TEntity>, new();

    Task CommitAsync();
}