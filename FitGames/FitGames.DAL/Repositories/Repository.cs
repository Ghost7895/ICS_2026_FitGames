using FitGames.DAL.Entities;
using FitGames.DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Repositories;

public class Repository<TEntity>(
    DbContext dbContext,
    IEntityMapper<TEntity> entityMapper)
    : IRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<string>? includePaths = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();
        if (includePaths is not null)
        {
            foreach (string path in includePaths)
            {
                query = query.Include(path);
            }
        }
        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, IEnumerable<string>? includePaths = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();
        if (includePaths is not null)
        {
            foreach (string path in includePaths)
            {
                query = query.Include(path);
            }
        }
        return await query.SingleOrDefaultAsync(e => e.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async ValueTask<bool> ExistAsync(TEntity entity, CancellationToken cancellationToken = default)
        => entity.Id != Guid.Empty
        && await _dbSet.AnyAsync(e => e.Id == entity.Id, cancellationToken).ConfigureAwait(false);

    public Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var inserted = _dbSet.Add(entity).Entity;
        return Task.FromResult(inserted);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        TEntity? existingEntity = await _dbSet
            .SingleOrDefaultAsync(e => e.Id == entity.Id, cancellationToken)
            .ConfigureAwait(false);
        if (existingEntity is null)
        {
            throw new EntityNotFoundException(typeof(TEntity), entity.Id);
        }

        entityMapper.MapToExistingEntity(existingEntity, entity);
        return existingEntity;
    }

    public async Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
    {
        TEntity? entity = await _dbSet
            .SingleOrDefaultAsync(i => i.Id == entityId, cancellationToken)
            .ConfigureAwait(false);
        if (entity is null)
        {
            throw new EntityNotFoundException(typeof(TEntity), entityId);
        }

        _dbSet.Remove(entity);
    }
}
