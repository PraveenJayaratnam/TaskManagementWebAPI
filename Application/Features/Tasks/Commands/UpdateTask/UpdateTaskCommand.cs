using Application.Common;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Tasks.Commands;

public record UpdateTaskCommand(Guid Id, string Title, string? Description, TaskItemStatus Status, string? DueDate) 
    : IRequest<Result<TaskDto>>;

