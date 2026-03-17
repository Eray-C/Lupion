using Empty_ERP_Template.Business.Exceptions;
using Empty_ERP_Template.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Empty_ERP_Template.Business.Repository;

public class Repository<T> where T : class
{
    protected readonly DBContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(DBContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // Tek kayýt (predicate zorunlu deðil), include ve tracking opsiyonlarý
    public async Task<T?> GetAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;

        if (asNoTracking)
            query = query.AsNoTracking();

        if (include != null)
            query = include(query);

        if (predicate != null)
            query = query.Where(predicate);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<T>> GetListAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        int? skip = null,
        int? take = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;

        if (asNoTracking)
            query = query.AsNoTracking();

        if (include != null)
            query = include(query);

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            query = orderBy(query);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public IQueryable<T> Query(bool asNoTracking = true)
        => asNoTracking ? _dbSet.AsNoTracking() : _dbSet;

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        => await _dbSet.AddRangeAsync(entities, cancellationToken);

    public void Update(T entity) => _dbSet.Update(entity);

    public void UpdateRange(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);

    public void Remove(T entity) => _dbSet.Remove(entity);
    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id)
                     ?? throw new RecordNotFoundException();

        var prop = entity.GetType().GetProperty("IsDeleted");
        if (prop != null && prop.PropertyType == typeof(bool))
        {
            prop.SetValue(entity, true);
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException($"{typeof(T).Name} entity'sinde IsDeleted alaný yok.");
        }
    }

    public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.AnyAsync(predicate, cancellationToken);

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        => predicate == null
            ? await _dbSet.CountAsync(cancellationToken)
            : await _dbSet.CountAsync(predicate, cancellationToken);
}
