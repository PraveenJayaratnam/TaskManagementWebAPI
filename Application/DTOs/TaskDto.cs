using Domain.Enums;

namespace Application.DTOs;

public class TaskDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public DateTimeOffset? DueDate { get; set; }
}

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
    public TaskPriority Priority { get; set; } = TaskPriority.Low;
    public DateTimeOffset DueDate { get; set; }
}

public class UpdateTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskItemStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public DateTimeOffset? DueDate { get; set; }
}

public class TaskFilterDto
{
    public Guid UserId { get; set; }
    public TaskItemStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public DateTimeOffset? DueDateFrom { get; set; }
    public DateTimeOffset? DueDateTo { get; set; }
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
}