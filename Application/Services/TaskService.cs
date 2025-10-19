using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.UnitOfWork;

namespace Application.Services;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskDto?> GetByIdAsync(Guid id)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        return task != null ? task.Adapt<TaskDto>() : null;
    }

    public async Task<IQueryable<TaskDto>> GetAllAsync()
    {
        var query = await _unitOfWork.Tasks.GetAllQueryable();
        return await Task.FromResult(query.Select(t => t.Adapt<TaskDto>()));
    }

    public async Task<IQueryable<TaskDto>> GetByUserIdAsync(Guid userId)
    {
        var query = await _unitOfWork.Tasks.GetByUserIdQueryable(userId);
        return await Task.FromResult(query.Select(t => t.Adapt<TaskDto>()));
    }

    public async Task<TaskDto> CreateAsync(Guid userId, CreateTaskDto createTaskDto)
    {
        var task = createTaskDto.Adapt<TaskItem>();
        task.UserId = userId;
        
        _unitOfWork.Tasks.Add(task);
        await _unitOfWork.SaveChangesAsync();
        
        return task.Adapt<TaskDto>();
    }

    public async Task<TaskDto> UpdateAsync(Guid id, UpdateTaskDto updateTaskDto)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null)
        {
            throw new KeyNotFoundException("Task not found");
        }

        updateTaskDto.Adapt(task);
        _unitOfWork.Tasks.Update(task);
        await _unitOfWork.SaveChangesAsync();
        
        return task.Adapt<TaskDto>();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null) return false;

        _unitOfWork.Tasks.SoftDelete(task);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IQueryable<TaskDto>> GetFilteredAsync(TaskFilterDto filterDto)
    {
        var query = await _unitOfWork.Tasks.GetByUserIdQueryable(filterDto.UserId);

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

        return await Task.FromResult(query.Select(t => t.Adapt<TaskDto>()));
    }

    public async Task<DataResponse<TaskDto>> GetFilteredPaginatedAsync(TaskFilterDto filterDto)
    {
        var query = await _unitOfWork.Tasks.GetByUserIdQueryable(filterDto.UserId);
        
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

        var totalCount = await query.CountAsync();

        if (!string.IsNullOrEmpty(filterDto.SortBy))
        {
            query = ApplySorting(query, filterDto.SortBy, filterDto.SortDirection);
        }
        else
        {
            query = query.OrderByDescending(t => t.CreatedAt);
        }

        var paginatedTasks = await query
            .Skip(filterDto.PageIndex * filterDto.PageSize)
            .Take(filterDto.PageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling((double)totalCount / filterDto.PageSize);

        return new DataResponse<TaskDto>
        {
            Items = paginatedTasks.Adapt<List<TaskDto>>(),
            PageSize = filterDto.PageSize,
            PageIndex = filterDto.PageIndex,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = filterDto.PageIndex > 0,
            HasNextPage = filterDto.PageIndex < totalPages - 1
        };
    }

    private static IQueryable<TaskItem> ApplySorting(IQueryable<TaskItem> query, string sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";
        
        return sortBy?.ToLowerInvariant() switch
        {
            "title" => isDescending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
            "status" => isDescending ? query.OrderByDescending(t => t.Status) : query.OrderBy(t => t.Status),
            "priority" => isDescending ? query.OrderByDescending(t => t.Priority) : query.OrderBy(t => t.Priority),
            "createdat" => isDescending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt),
            "duedate" => isDescending ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };
    }

    private static object GetPropertyValue(TaskItem task, string propertyName)
    {
        return propertyName?.ToLowerInvariant() switch
        {
            "title" => task.Title ?? string.Empty,
            "status" => task.Status,
            "createdat" => task.CreatedAt,
            "duedate" => task.DueDate ?? DateTime.MinValue,
            _ => task.CreatedAt
        };
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _unitOfWork.Tasks.ExistsAsync(t => t.Id == id);
    }

    public async Task<bool> BelongsToUserAsync(Guid taskId, Guid userId)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(taskId);
        return task != null && task.UserId == userId;
    }

}

