using BffPortalService.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace BffPortalService.Infrastructure.Database.Repositories;

public class UnitOfWork
{
    private readonly BffPortalDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(BffPortalDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await _context.SaveChangesAsync(ct);
    }

    public async Task BeginTransactionAsync(CancellationToken ct)
    {
        _transaction = await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct)
    {
        try
        {
            await _context.SaveChangesAsync(ct);
            if (_transaction is not null)
            {
                await _transaction.CommitAsync(ct);
            }
        }
        finally
        {
            if (_transaction is not null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackAsync(CancellationToken ct)
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
