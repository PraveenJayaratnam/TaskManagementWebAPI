using Application.DTOs;
using Application.Services;
using MediatR;

namespace Application.Features.Tasks.Queries.GetTasksQueryable;

public class GetTasksQueryableQueryHandler(ITaskService taskService) : IRequestHandler<GetTasksQueryableQuery, IQueryable<TaskDto>>
{

    public async Task<IQueryable<TaskDto>> Handle(GetTasksQueryableQuery request, CancellationToken cancellationToken)
    {
        if (request.FilterDto == null)
        {
            return await taskService.GetAllAsync();
        }
        else
        {
            return await taskService.GetFilteredAsync(request.FilterDto);
        }
    }
}
