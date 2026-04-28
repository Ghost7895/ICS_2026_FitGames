using System.Linq.Expressions;
using FitGames.DAL.Entities;

namespace FitGames.DAL.Repositories;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        IEnumerable<string>? includePaths = null,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(Guid id, IEnumerable<string>? includePaths = null, CancellationToken cancellationToken = default);
    ValueTask<bool> ExistAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default);
}