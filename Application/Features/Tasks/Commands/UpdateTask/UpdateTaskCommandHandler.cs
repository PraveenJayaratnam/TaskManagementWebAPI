using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Tasks.Commands;

public class UpdateTaskCommandHandler(ITaskService taskService) : IRequestHandler<UpdateTaskCommand, Result<TaskDto>>
{

    public async Task<Result<TaskDto>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var taskDto = await taskService.UpdateAsync(request.Id, request.UpdateTaskDto);
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
