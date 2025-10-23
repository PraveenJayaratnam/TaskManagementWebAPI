using System.Linq.Expressions;
using Infrastructure.Data;
using Domain.Entities;
using Application.Services;
using Infrastructure.UnitOfWork;

namespace Infrastructure.Repositories;

public class GenericRepository<T>(ApplicationDbContext context, IAuditService auditService, IUnitOfWork unitOfWork) : IGenericRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext context = context;
    protected readonly DbSet<T> dbSet = context.Set<T>();
    protected readonly IAuditService auditService = auditService;
    protected readonly IUnitOfWork unitOfWork = unitOfWork;

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await dbSet
            .AsNoTracking()
            .Where(e => e.Id == id && !e.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public virtual async Task<IQueryable<T>> GetAllAsync()
    {
        return await Task.FromResult(dbSet
            .AsNoTracking()
            .Where(e => !e.IsDeleted));
    }

    public virtual async Task<IQueryable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await Task.FromResult(dbSet
            .AsNoTracking()
            .Where(e => !e.IsDeleted)
            .Where(predicate));
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await dbSet
            .AsNoTracking()
            .Where(e => !e.IsDeleted)
            .FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await dbSet
            .AsNoTracking()
            .Where(e => !e.IsDeleted)
            .AnyAsync(predicate);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        var query = dbSet.AsNoTracking().Where(e => !e.IsDeleted);
        
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        
        return await query.CountAsync();
    }

    public virtual async Task<IQueryable<T>> GetAllQueryable()
    {
        return await Task.FromResult(dbSet.AsNoTracking().Where(e => !e.IsDeleted));
    }

    public virtual async Task<IQueryable<T>> FindQueryable(Expression<Func<T, bool>> predicate)
    {
        return await Task.FromResult(dbSet.AsNoTracking().Where(e => !e.IsDeleted).Where(predicate));
    }


    public virtual async Task AddAsync(T entity)
    {
        var currentUserId = auditService.GetCurrentUserId();
        var now = DateTimeOffset.UtcNow;
        
        entity.CreatedAt = now;
        entity.CreatedById = currentUserId;
        entity.IsActive = true;
        entity.IsDeleted = false;
        
        dbSet.Add(entity);
        await Task.CompletedTask;
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        var currentUserId = auditService.GetCurrentUserId();
        var now = DateTimeOffset.UtcNow;
        
        foreach (var entity in entities)
        {
            entity.CreatedAt = now;
            entity.CreatedById = currentUserId;
            entity.IsActive = true;
            entity.IsDeleted = false;
        }
        dbSet.AddRange(entities);
        await Task.CompletedTask;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        var currentUserId = auditService.GetCurrentUserId();
        var now = DateTimeOffset.UtcNow;
        
        entity.UpdatedAt = now;
        entity.UpdatedById = currentUserId;
        
        dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        var currentUserId = auditService.GetCurrentUserId();
        var now = DateTimeOffset.UtcNow;
        
        foreach (var entity in entities)
        {
            entity.UpdatedAt = now;
            entity.UpdatedById = currentUserId;
        }
        dbSet.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public virtual async Task RemoveAsync(T entity)
    {
        dbSet.Remove(entity);
        await Task.CompletedTask;
    }

    public virtual async Task RemoveRangeAsync(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
        await Task.CompletedTask;
    }

    public virtual async Task SoftDeleteAsync(T entity)
    {
        var currentUserId = auditService.GetCurrentUserId();
        var now = DateTimeOffset.UtcNow;
        
        entity.IsDeleted = true;
        entity.UpdatedAt = now;
        entity.UpdatedById = currentUserId;
        
        dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public virtual async Task SoftDeleteRangeAsync(IEnumerable<T> entities)
    {
        var currentUserId = auditService.GetCurrentUserId();
        var now = DateTimeOffset.UtcNow;
        
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = now;
            entity.UpdatedById = currentUserId;
        }
        dbSet.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public virtual async Task SaveChangesAsync()
    {
        await unitOfWork.SaveChangesAsync();
    }


    public virtual async Task<IQueryable<T>> GetPaginatedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
    {
        return await Task.FromResult(dbSet
            .AsNoTracking()
            .Where(e => !e.IsDeleted)
            .Where(predicate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize));
    }

    public virtual async Task<IQueryable<T>> GetPaginatedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, Expression<Func<T, object>> orderBy)
    {
        return await Task.FromResult(dbSet
            .AsNoTracking()
            .Where(e => !e.IsDeleted)
            .Where(predicate)
            .OrderBy(orderBy)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize));
    }

    public virtual async Task<IQueryable<T>> GetPaginatedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, Expression<Func<T, object>> orderBy, bool isDescending)
    {
        var query = dbSet
            .AsNoTracking()
            .Where(e => !e.IsDeleted)
            .Where(predicate);

        if (isDescending)
        {
            query = query.OrderByDescending(orderBy);
        }
        else
        {
            query = query.OrderBy(orderBy);
        }

        return await Task.FromResult(query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize));
    }


    public virtual async Task<IQueryable<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, bool>> additionalFilter)
    {
        return await Task.FromResult(dbSet
            .AsNoTracking()
            .Where(e => !e.IsDeleted)
            .Where(predicate)
            .Where(additionalFilter));
    }

    public virtual async Task<IQueryable<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, bool>> additionalFilter, Expression<Func<T, object>> orderBy)
    {
        return await Task.FromResult(dbSet
            .AsNoTracking()
            .Where(e => !e.IsDeleted)
            .Where(predicate)
            .Where(additionalFilter)
            .OrderBy(orderBy));
    }

    public virtual async Task<IQueryable<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, bool>> additionalFilter, Expression<Func<T, object>> orderBy, bool isDescending)
    {
        var query = dbSet
            .AsNoTracking()
            .Where(e => !e.IsDeleted)
            .Where(predicate)
            .Where(additionalFilter);

        if (isDescending)
        {
            query = query.OrderByDescending(orderBy);
        }
        else
        {
            query = query.OrderBy(orderBy);
        }

        return await Task.FromResult(query);
    }

}

