using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Tasks.Queries;

public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, Result<IEnumerable<TaskDto>>>
{
    private readonly ITaskService _taskService;

    public GetAllTasksQueryHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<Result<IEnumerable<TaskDto>>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tasks = await _taskService.GetAllAsync();
            return Result<IEnumerable<TaskDto>>.Success(tasks);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<TaskDto>>.Failure($"Get all tasks failed: {ex.Message}");
        }
    }
}
