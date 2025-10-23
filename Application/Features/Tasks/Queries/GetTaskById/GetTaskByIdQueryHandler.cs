using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Tasks.Queries;

public class GetTaskByIdQueryHandler(ITaskService taskService) : IRequestHandler<GetTaskByIdQuery, Result<TaskDto>>
{

    public async Task<Result<TaskDto>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var task = await taskService.GetByIdAsync(request.Id);
            if (task == null)
            {
                return Result<TaskDto>.Failure("Task not found");
            }

            return Result<TaskDto>.Success(task);
        }
        catch (Exception ex)
        {
            return Result<TaskDto>.Failure($"Get task failed: {ex.Message}");
        }
    }
}

