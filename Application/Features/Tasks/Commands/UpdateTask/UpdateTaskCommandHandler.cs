using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Tasks.Commands;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result<TaskDto>>
{
    private readonly ITaskService _taskService;

    public UpdateTaskCommandHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<Result<TaskDto>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var updateTaskDto = new UpdateTaskDto
            {
                Title = request.Title,
                Description = request.Description,
                Status = request.Status.ToString(),
                DueDate = request.DueDate
            };

            var taskDto = await _taskService.UpdateAsync(request.Id, updateTaskDto);
            return Result<TaskDto>.Success(taskDto);
        }
        catch (KeyNotFoundException ex)
        {
            return Result<TaskDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result<TaskDto>.Failure($"Update task failed: {ex.Message}");
        }
    }
}
