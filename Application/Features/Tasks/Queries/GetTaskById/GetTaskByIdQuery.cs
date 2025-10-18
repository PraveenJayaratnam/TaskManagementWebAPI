using Application.Common;
using Application.DTOs;

namespace Application.Features.Tasks.Queries;

public record GetTaskByIdQuery(Guid Id) : IRequest<Result<TaskDto>>;

