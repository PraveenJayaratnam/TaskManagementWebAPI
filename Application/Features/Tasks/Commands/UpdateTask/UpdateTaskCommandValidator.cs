namespace Application.Features.Tasks.Commands;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Task ID is required");

        RuleFor(x => x.UpdateTaskDto.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.UpdateTaskDto.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.UpdateTaskDto.Status)
            .IsInEnum().WithMessage("Status must be a valid TaskItemStatus value");

        RuleFor(x => x.UpdateTaskDto.Priority)
            .IsInEnum().WithMessage("Priority must be a valid TaskPriority value");

        RuleFor(x => x.UpdateTaskDto.DueDate)
            .NotEmpty().WithMessage("Due date is required")
            .When(x => x.UpdateTaskDto.DueDate != null);
    }

}

