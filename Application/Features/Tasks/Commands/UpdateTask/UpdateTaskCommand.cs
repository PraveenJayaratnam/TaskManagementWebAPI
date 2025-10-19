using Application.Common;
using Application.DTOs;

namespace Application.Features.Tasks.Commands;

public record UpdateTaskCommand(Guid Id, UpdateTaskDto UpdateTaskDto) 
    : IRequest<Result<TaskDto>>;

