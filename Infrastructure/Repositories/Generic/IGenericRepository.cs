using System.Linq.Expressions;
using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IQueryable<T>> GetAllAsync();
    Task<IQueryable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

    Task<IQueryable<T>> GetAllQueryable();
    Task<IQueryable<T>> FindQueryable(Expression<Func<T, bool>> predicate);
    
    Task<IQueryable<T>> GetPaginatedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize);
    Task<IQueryable<T>> GetPaginatedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, Expression<Func<T, object>> orderBy);
    Task<IQueryable<T>> GetPaginatedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, Expression<Func<T, object>> orderBy, bool isDescending);
    
    Task<IQueryable<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, bool>> additionalFilter);
    Task<IQueryable<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, bool>> additionalFilter, Expression<Func<T, object>> orderBy);
    Task<IQueryable<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, bool>> additionalFilter, Expression<Func<T, object>> orderBy, bool isDescending);

    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(IEnumerable<T> entities);
    Task RemoveAsync(T entity);
    Task RemoveRangeAsync(IEnumerable<T> entities);
    Task SoftDeleteAsync(T entity);
    Task SoftDeleteRangeAsync(IEnumerable<T> entities);
    Task SaveChangesAsync();

}

