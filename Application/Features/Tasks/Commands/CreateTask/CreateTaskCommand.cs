using Application.Common;
using Application.DTOs;

namespace Application.Features.Tasks.Commands;

public record CreateTaskCommand(Guid UserId, string Title, string? Description, string? DueDate) 
    : IRequest<Result<TaskDto>>;

