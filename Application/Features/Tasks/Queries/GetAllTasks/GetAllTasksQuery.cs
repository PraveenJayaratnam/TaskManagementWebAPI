using Application.Common;
using Application.DTOs;

namespace Application.Features.Tasks.Queries;

public record GetAllTasksQuery() : IRequest<Result<IEnumerable<TaskDto>>>;

