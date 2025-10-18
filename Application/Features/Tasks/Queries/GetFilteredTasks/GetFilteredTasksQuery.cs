using Application.Common;
using Application.DTOs;

namespace Application.Features.Tasks.Queries;

public record GetFilteredTasksQuery(TaskFilterDto Filter) : IRequest<Result<DataResponse<TaskDto>>>;
