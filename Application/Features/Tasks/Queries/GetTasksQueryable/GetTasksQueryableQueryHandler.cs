using Application.DTOs;
using Application.Services;
using MediatR;

namespace Application.Features.Tasks.Queries.GetTasksQueryable;

public class GetTasksQueryableQueryHandler : IRequestHandler<GetTasksQueryableQuery, IQueryable<TaskDto>>
{
    private readonly ITaskService _taskService;

    public GetTasksQueryableQueryHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<IQueryable<TaskDto>> Handle(GetTasksQueryableQuery request, CancellationToken cancellationToken)
    {
        if (request.FilterDto == null)
        {
            return await _taskService.GetAllAsync();
        }
        else
        {
            return await _taskService.GetFilteredAsync(request.FilterDto);
        }
    }
}
