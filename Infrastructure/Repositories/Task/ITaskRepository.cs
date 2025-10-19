using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Repositories;

public interface ITaskRepository : IGenericRepository<TaskItem>
{
    Task<IEnumerable<TaskItem>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<TaskItem>> GetByStatusAsync(TaskItemStatus status);
    Task<IEnumerable<TaskItem>> GetByPriorityAsync(TaskPriority priority);
    Task<IEnumerable<TaskItem>> GetByDueDateRangeAsync(DateTime from, DateTime to);
    Task<IEnumerable<TaskItem>> SearchAsync(string searchTerm);
    Task<int> GetCountByUserIdAsync(Guid userId);
    Task<IEnumerable<TaskItem>> GetPaginatedAsync(Guid userId, int pageNumber, int pageSize);
    Task<IQueryable<TaskItem>> GetByUserIdQueryable(Guid userId);
    Task<IQueryable<TaskItem>> GetByStatusQueryable(TaskItemStatus status);
    Task<IQueryable<TaskItem>> GetByPriorityQueryable(TaskPriority priority);
    Task<IQueryable<TaskItem>> GetByDueDateRangeQueryable(DateTime from, DateTime to);
    Task<IQueryable<TaskItem>> SearchQueryable(string searchTerm);
}

