using Application.Common;
using Application.DTOs;

namespace Application.Features.Tasks.Queries;

public record GetTasksByUserQuery(Guid UserId) : IRequest<Result<IEnumerable<TaskDto>>>;

