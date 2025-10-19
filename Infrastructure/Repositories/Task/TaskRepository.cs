using Infrastructure.Data;
using Domain.Entities;
using Domain.Enums;
using Application.Services;

namespace Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<TaskItem>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext context, IAuditService auditService) : base(context, auditService)
        {
        }

        public async Task<IEnumerable<TaskItem>> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Include(t => t.User)
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByStatusAsync(TaskItemStatus status)
        {
            return await _dbSet
                .Include(t => t.User)
                .Where(t => t.Status == status && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByPriorityAsync(TaskPriority priority)
        {
            return await _dbSet
                .Include(t => t.User)
                .Where(t => t.Priority == priority && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByDueDateRangeAsync(DateTime from, DateTime to)
        {
            return await _dbSet
                .Include(t => t.User)
                .Where(t => t.DueDate >= from && t.DueDate <= to && !t.IsDeleted)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> SearchAsync(string searchTerm)
        {
            return await _dbSet
                .Include(t => t.User)
                .Where(t => (t.Title.Contains(searchTerm) || 
                           (t.Description != null && t.Description.Contains(searchTerm))) 
                           && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetCountByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .CountAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetPaginatedAsync(Guid userId, int pageNumber, int pageSize)
        {
            return await _dbSet
                .Include(t => t.User)
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(t => t.User)
                .Where(t => t.Id == id && !t.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public override async Task<IQueryable<TaskItem>> GetAllAsync()
        {
            return await Task.FromResult(_dbSet
                .Include(t => t.User)
                .Where(t => !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt));
        }

        public override void Update(TaskItem entity)
        {
            if (entity.Status == TaskItemStatus.Completed && entity.CompletedAt == null)
            {
                entity.CompletedAt = DateTime.UtcNow;
            }
            base.Update(entity);
        }

        public async Task<IQueryable<TaskItem>> GetByUserIdQueryable(Guid userId)
        {
            return await Task.FromResult(_dbSet
                .Include(t => t.User)
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt));
        }

        public async Task<IQueryable<TaskItem>> GetByStatusQueryable(TaskItemStatus status)
        {
            return await Task.FromResult(_dbSet
                .Include(t => t.User)
                .Where(t => t.Status == status && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt));
        }

        public async Task<IQueryable<TaskItem>> GetByPriorityQueryable(TaskPriority priority)
        {
            return await Task.FromResult(_dbSet
                .Include(t => t.User)
                .Where(t => t.Priority == priority && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt));
        }

        public async Task<IQueryable<TaskItem>> GetByDueDateRangeQueryable(DateTime from, DateTime to)
        {
            return await Task.FromResult(_dbSet
                .Include(t => t.User)
                .Where(t => t.DueDate >= from && t.DueDate <= to && !t.IsDeleted)
                .OrderBy(t => t.DueDate));
        }

        public async Task<IQueryable<TaskItem>> SearchQueryable(string searchTerm)
        {
            return await Task.FromResult(_dbSet
                .Include(t => t.User)
                .Where(t => (t.Title.Contains(searchTerm) || 
                           (t.Description != null && t.Description.Contains(searchTerm))) 
                           && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt));
        }
    }
}

