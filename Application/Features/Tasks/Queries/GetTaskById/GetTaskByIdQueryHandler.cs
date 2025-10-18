using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Tasks.Queries;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, Result<TaskDto>>
{
    private readonly ITaskService _taskService;

    public GetTaskByIdQueryHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<Result<TaskDto>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var task = await _taskService.GetByIdAsync(request.Id);
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

