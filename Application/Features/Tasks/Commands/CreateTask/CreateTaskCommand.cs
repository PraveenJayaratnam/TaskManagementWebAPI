using Application.Common;
using Application.DTOs;

namespace Application.Features.Tasks.Commands;

public record CreateTaskCommand(Guid UserId, CreateTaskDto CreateTaskDto) 
    : IRequest<Result<TaskDto>>;

