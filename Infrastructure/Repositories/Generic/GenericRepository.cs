using System.Linq.Expressions;
using Infrastructure.Data;
using Domain.Entities;
using Application.Services;

namespace Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;
    protected readonly IAuditService _auditService;

    public GenericRepository(ApplicationDbContext context, IAuditService auditService)
    {
        _context = context;
        _dbSet = context.Set<T>();
        _auditService = auditService;
    }

    #region Query Operations

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Where(e => e.Id == id && !e.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet
            .Where(e => !e.IsDeleted)
            .ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .Where(e => !e.IsDeleted)
            .Where(predicate)
            .ToListAsync();
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .Where(e => !e.IsDeleted)
            .FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .Where(e => !e.IsDeleted)
            .AnyAsync(predicate);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        var query = _dbSet.Where(e => !e.IsDeleted);
        
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        
        return await query.CountAsync();
    }

    #endregion

    #region Command Operations

    public virtual void Add(T entity)
    {
        var currentUserId = _auditService.GetCurrentUserId();
        var now = DateTime.UtcNow;
        
        entity.CreatedAt = now;
        entity.CreatedById = currentUserId;
        entity.IsActive = true;
        entity.IsDeleted = false;
        
        _dbSet.Add(entity);
    }

    public virtual void AddRange(IEnumerable<T> entities)
    {
        var currentUserId = _auditService.GetCurrentUserId();
        var now = DateTime.UtcNow;
        
        foreach (var entity in entities)
        {
            entity.CreatedAt = now;
            entity.CreatedById = currentUserId;
            entity.IsActive = true;
            entity.IsDeleted = false;
        }
        _dbSet.AddRange(entities);
    }

    public virtual void Update(T entity)
    {
        var currentUserId = _auditService.GetCurrentUserId();
        var now = DateTime.UtcNow;
        
        entity.UpdatedAt = now;
        entity.UpdatedById = currentUserId;
        
        _dbSet.Update(entity);
    }

    public virtual void UpdateRange(IEnumerable<T> entities)
    {
        var currentUserId = _auditService.GetCurrentUserId();
        var now = DateTime.UtcNow;
        
        foreach (var entity in entities)
        {
            entity.UpdatedAt = now;
            entity.UpdatedById = currentUserId;
        }
        _dbSet.UpdateRange(entities);
    }

    public virtual void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public virtual void SoftDelete(T entity)
    {
        var currentUserId = _auditService.GetCurrentUserId();
        var now = DateTime.UtcNow;
        
        entity.IsDeleted = true;
        entity.UpdatedAt = now;
        entity.UpdatedById = currentUserId;
        
        _dbSet.Update(entity);
    }

    public virtual void SoftDeleteRange(IEnumerable<T> entities)
    {
        var currentUserId = _auditService.GetCurrentUserId();
        var now = DateTime.UtcNow;
        
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = now;
            entity.UpdatedById = currentUserId;
        }
        _dbSet.UpdateRange(entities);
    }

    #endregion

    #region Save Operations

    public virtual async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    #endregion
}

