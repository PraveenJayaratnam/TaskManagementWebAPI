using Application.DTOs;

namespace Application.Services;

public interface ITaskService
{
    Task<TaskDto?> GetByIdAsync(Guid id);
    Task<IQueryable<TaskDto>> GetAllAsync();
    Task<IQueryable<TaskDto>> GetByUserIdAsync(Guid userId);
    Task<TaskDto> CreateAsync(Guid userId, CreateTaskDto createTaskDto);
    Task<TaskDto> UpdateAsync(Guid id, UpdateTaskDto updateTaskDto);
    Task<bool> DeleteAsync(Guid id);
    Task<IQueryable<TaskDto>> GetFilteredAsync(TaskFilterDto filterDto);
    Task<DataResponse<TaskDto>> GetFilteredPaginatedAsync(TaskFilterDto filterDto);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> BelongsToUserAsync(Guid taskId, Guid userId);
}

