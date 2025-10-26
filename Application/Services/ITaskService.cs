using Application.DTOs;

namespace Application.Services;

public interface ITaskService
{
    Task<TaskDto?> GetByIdAsync(Guid id);
    Task<IQueryable<TaskDto>> GetAllAsync();
    Task<TaskDto> CreateAsync(Guid userId, CreateTaskDto createTaskDto);
    Task<TaskDto> UpdateAsync(Guid id, UpdateTaskDto updateTaskDto);
    Task<bool> DeleteAsync(Guid id);
    Task<DataResponse<TaskDto>> GetFilteredPaginatedAsync(TaskFilterDto filterDto);
    Task<bool> ExistsAsync(Guid id);
}

