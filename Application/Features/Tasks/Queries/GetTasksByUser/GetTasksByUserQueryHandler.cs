using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Tasks.Queries;

public class GetTasksByUserQueryHandler : IRequestHandler<GetTasksByUserQuery, Result<IEnumerable<TaskDto>>>
{
    private readonly ITaskService _taskService;

    public GetTasksByUserQueryHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<Result<IEnumerable<TaskDto>>> Handle(GetTasksByUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tasks = await _taskService.GetByUserIdAsync(request.UserId);
            return Result<IEnumerable<TaskDto>>.Success(tasks);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<TaskDto>>.Failure($"Get tasks by user failed: {ex.Message}");
        }
    }
}

