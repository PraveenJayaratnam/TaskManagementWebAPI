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

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await taskRepository.ExistsAsync(t => t.Id == id);
    }
}

