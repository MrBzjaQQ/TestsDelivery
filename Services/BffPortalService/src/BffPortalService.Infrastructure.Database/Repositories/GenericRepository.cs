using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BffPortalService.Infrastructure.Database.Repositories;

public class GenericRepository<T> where T : class
{
    protected readonly DbContext Context;
    protected readonly DbSet<T> DbSet;

    public GenericRepository(DbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await DbSet.FindAsync([id], ct);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct)
    {
        return await DbSet.ToListAsync(ct);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct)
    {
        return await DbSet.Where(predicate).ToListAsync(ct);
    }

    public virtual async Task AddAsync(T entity, CancellationToken ct)
    {
        await DbSet.AddAsync(entity, ct);
    }

    public virtual void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        DbSet.Remove(entity);
    }
}
