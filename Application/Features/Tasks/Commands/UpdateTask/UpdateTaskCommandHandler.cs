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
            var taskDto = await _taskService.UpdateAsync(request.Id, request.UpdateTaskDto);
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
