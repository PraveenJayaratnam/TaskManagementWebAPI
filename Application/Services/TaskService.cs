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

    public async Task<IEnumerable<TaskDto>> GetAllAsync()
    {
        var tasks = await _unitOfWork.Tasks.GetAllAsync();
        return tasks.Adapt<IEnumerable<TaskDto>>();
    }

    public async Task<IEnumerable<TaskDto>> GetByUserIdAsync(Guid userId)
    {
        var tasks = await _unitOfWork.Tasks.GetByUserIdAsync(userId);
        return tasks.Adapt<IEnumerable<TaskDto>>();
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

    public async Task<IEnumerable<TaskDto>> GetFilteredAsync(TaskFilterDto filterDto)
    {
        var tasks = await _unitOfWork.Tasks.GetByUserIdAsync(filterDto.UserId);
        
        if (!string.IsNullOrEmpty(filterDto.Status))
        {
            if (Enum.TryParse<TaskItemStatus>(filterDto.Status, out var status))
            {
                tasks = tasks.Where(t => t.Status == status);
            }
        }
        
        if (filterDto.DueDateFrom.HasValue)
        {
            tasks = tasks.Where(t => t.DueDate >= filterDto.DueDateFrom.Value);
        }
        
        if (filterDto.DueDateTo.HasValue)
        {
            tasks = tasks.Where(t => t.DueDate <= filterDto.DueDateTo.Value);
        }
        
        if (!string.IsNullOrEmpty(filterDto.SearchTerm))
        {
            tasks = tasks.Where(t => t.Title.Contains(filterDto.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   (t.Description != null && t.Description.Contains(filterDto.SearchTerm, StringComparison.OrdinalIgnoreCase)));
        }

        if (!string.IsNullOrEmpty(filterDto.SortBy))
        {
            tasks = filterDto.SortDirection?.ToLower() == "desc" 
                ? tasks.OrderByDescending(t => GetPropertyValue(t, filterDto.SortBy))
                : tasks.OrderBy(t => GetPropertyValue(t, filterDto.SortBy));
        }
        else
        {
            tasks = tasks.OrderByDescending(t => t.CreatedAt);
        }

        return tasks.Adapt<IEnumerable<TaskDto>>();
    }

    public async Task<DataResponse<TaskDto>> GetFilteredPaginatedAsync(TaskFilterDto filterDto)
    {
        var tasks = await _unitOfWork.Tasks.GetByUserIdAsync(filterDto.UserId);
        
        if (!string.IsNullOrEmpty(filterDto.Status))
        {
            if (Enum.TryParse<TaskItemStatus>(filterDto.Status, out var status))
            {
                tasks = tasks.Where(t => t.Status == status);
            }
        }
        
        if (filterDto.DueDateFrom.HasValue)
        {
            tasks = tasks.Where(t => t.DueDate >= filterDto.DueDateFrom.Value);
        }
        
        if (filterDto.DueDateTo.HasValue)
        {
            tasks = tasks.Where(t => t.DueDate <= filterDto.DueDateTo.Value);
        }
        
        if (!string.IsNullOrEmpty(filterDto.SearchTerm))
        {
            tasks = tasks.Where(t => t.Title.Contains(filterDto.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   (t.Description != null && t.Description.Contains(filterDto.SearchTerm, StringComparison.OrdinalIgnoreCase)));
        }

        var totalCount = tasks.Count();

        if (!string.IsNullOrEmpty(filterDto.SortBy))
        {
            tasks = filterDto.SortDirection?.ToLower() == "desc" 
                ? tasks.OrderByDescending(t => GetPropertyValue(t, filterDto.SortBy))
                : tasks.OrderBy(t => GetPropertyValue(t, filterDto.SortBy));
        }
        else
        {
            tasks = tasks.OrderByDescending(t => t.CreatedAt);
        }

        var paginatedTasks = tasks
            .Skip(filterDto.PageIndex * filterDto.PageSize)
            .Take(filterDto.PageSize)
            .ToList();

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

