using Application.DTOs;
using Domain.Entities;
using Infrastructure.Repositories;
using System.Linq.Expressions;

namespace Application.Services;

public class TaskService(IGenericRepository<TaskItem> taskRepository) : ITaskService
{

    public async Task<TaskDto?> GetByIdAsync(Guid id)
    {
        var task = await taskRepository.GetByIdAsync(id);
        return task != null ? task.Adapt<TaskDto>() : null;
    }

    public async Task<IQueryable<TaskDto>> GetAllAsync()
    {
        var query = await taskRepository.GetAllQueryable();
        return query.Select(t => t.Adapt<TaskDto>());
    }

    public async Task<IQueryable<TaskDto>> GetByUserIdAsync(Guid userId)
    {
        var query = await taskRepository.FindQueryable(t => t.UserId == userId);
        return query.Select(t => t.Adapt<TaskDto>());
    }

    public async Task<TaskDto> CreateAsync(Guid userId, CreateTaskDto createTaskDto)
    {
        var task = createTaskDto.Adapt<TaskItem>();
        task.UserId = userId;
        
        await taskRepository.AddAsync(task);
        await taskRepository.SaveChangesAsync();
        
        return task.Adapt<TaskDto>();
    }

    public async Task<TaskDto> UpdateAsync(Guid id, UpdateTaskDto updateTaskDto)
    {
        var task = await taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            throw new KeyNotFoundException("Task not found");
        }

        updateTaskDto.Adapt(task);
        await taskRepository.UpdateAsync(task);
        await taskRepository.SaveChangesAsync();
        
        return task.Adapt<TaskDto>();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var task = await taskRepository.GetByIdAsync(id);
        if (task == null) return false;

        await taskRepository.SoftDeleteAsync(task);
        await taskRepository.SaveChangesAsync();
        return true;
    }

    public async Task<IQueryable<TaskDto>> GetFilteredAsync(TaskFilterDto filterDto)
    {
        var query = await taskRepository.FindQueryable(t => t.UserId == filterDto.UserId);

        if (filterDto.Status.HasValue)
        {
            query = query.Where(t => t.Status == filterDto.Status.Value);
        }
        
        if (filterDto.Priority.HasValue)
        {
            query = query.Where(t => t.Priority == filterDto.Priority.Value);
        }
        
        if (filterDto.DueDateFrom.HasValue)
        {
            query = query.Where(t => t.DueDate >= filterDto.DueDateFrom.Value);
        }
        
        if (filterDto.DueDateTo.HasValue)
        {
            query = query.Where(t => t.DueDate <= filterDto.DueDateTo.Value);
        }
        
        if (!string.IsNullOrEmpty(filterDto.SearchTerm))
        {
            query = query.Where(t => t.Title.Contains(filterDto.SearchTerm) ||
                                   (t.Description != null && t.Description.Contains(filterDto.SearchTerm)));
        }

        if (!string.IsNullOrEmpty(filterDto.SortBy))
        {
            query = ApplySorting(query, filterDto.SortBy, filterDto.SortDirection);
        }
        else
        {
            query = query.OrderByDescending(t => t.CreatedAt);
        }

        return query.Select(t => t.Adapt<TaskDto>());
    }

    public async Task<DataResponse<TaskDto>> GetFilteredPaginatedAsync(TaskFilterDto filterDto)
    {
        var basePredicate = BuildOptimizedPredicate(filterDto);
        
        var countTask = taskRepository.CountAsync(basePredicate);
        var orderByExpression = GetOrderByExpression(filterDto.SortBy);
        
        var paginatedTasks = await taskRepository.GetPaginatedAsync(
            basePredicate,
            filterDto.PageIndex + 1,
            filterDto.PageSize,
            orderByExpression,
            filterDto.SortDirection?.ToLower() == "desc"
        );

        var totalCount = await countTask;
        var tasks = await paginatedTasks.ToListAsync();
        
        var totalPages = (int)Math.Ceiling((double)totalCount / filterDto.PageSize);

        return new DataResponse<TaskDto>
        {
            Items = tasks.Adapt<List<TaskDto>>(),
            PageSize = filterDto.PageSize,
            PageIndex = filterDto.PageIndex,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = filterDto.PageIndex > 0,
            HasNextPage = filterDto.PageIndex < totalPages - 1
        };
    }

    private Expression<Func<TaskItem, bool>> BuildOptimizedPredicate(TaskFilterDto filterDto)
    {
        return t => t.UserId == filterDto.UserId &&
                   (!filterDto.Status.HasValue || t.Status == filterDto.Status.Value) &&
                   (!filterDto.Priority.HasValue || t.Priority == filterDto.Priority.Value) &&
                   (!filterDto.DueDateFrom.HasValue || t.DueDate >= filterDto.DueDateFrom.Value) &&
                   (!filterDto.DueDateTo.HasValue || t.DueDate <= filterDto.DueDateTo.Value) &&
                   (string.IsNullOrEmpty(filterDto.SearchTerm) || 
                    t.Title.Contains(filterDto.SearchTerm) ||
                    (t.Description != null && t.Description.Contains(filterDto.SearchTerm)));
    }

    private Expression<Func<TaskItem, object>> GetOrderByExpression(string? sortBy)
    {
        return sortBy?.ToLowerInvariant() switch
        {
            "title" => t => t.Title,
            "status" => t => t.Status,
            "priority" => t => t.Priority,
            "createdat" => t => t.CreatedAt,
            "duedate" => t => t.DueDate,
            _ => t => t.CreatedAt
        };
    }

    private IQueryable<TaskItem> ApplySorting(IQueryable<TaskItem> query, string sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";
        
        return sortBy.ToLowerInvariant() switch
        {
            "title" => isDescending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
            "status" => isDescending ? query.OrderByDescending(t => t.Status) : query.OrderBy(t => t.Status),
            "priority" => isDescending ? query.OrderByDescending(t => t.Priority) : query.OrderBy(t => t.Priority),
            "createdat" => isDescending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt),
            "duedate" => isDescending ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };
    }


    public async Task<bool> ExistsAsync(Guid id)
    {
        return await taskRepository.ExistsAsync(t => t.Id == id);
    }

    public async Task<bool> BelongsToUserAsync(Guid taskId, Guid userId)
    {
        var task = await taskRepository.GetByIdAsync(taskId);
        return task != null && task.UserId == userId;
    }

}

