using Application.DTOs;

namespace Application.Features.Tasks.Queries.GetTasksQueryable;

public class GetTasksQueryableQuery : IRequest<IQueryable<TaskDto>>
{
    public TaskFilterDto? FilterDto { get; set; }

    public GetTasksQueryableQuery(TaskFilterDto? filterDto = null)
    {
        FilterDto = filterDto;
    }
}
