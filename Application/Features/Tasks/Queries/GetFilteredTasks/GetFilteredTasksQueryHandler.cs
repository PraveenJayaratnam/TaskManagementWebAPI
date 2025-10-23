using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Tasks.Queries;

public class GetFilteredTasksQueryHandler(ITaskService taskService) : IRequestHandler<GetFilteredTasksQuery, Result<DataResponse<TaskDto>>>
{

    public async Task<Result<DataResponse<TaskDto>>> Handle(GetFilteredTasksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await taskService.GetFilteredPaginatedAsync(request.Filter);
            return Result<DataResponse<TaskDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<DataResponse<TaskDto>>.Failure($"Get filtered tasks failed: {ex.Message}");
        }
    }
}
