namespace Application.Features.Tasks.Commands;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.CreateTaskDto.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.CreateTaskDto.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.CreateTaskDto.DueDate)
            .NotEmpty().WithMessage("Due date is required");

        RuleFor(x => x.CreateTaskDto.Status)
            .IsInEnum().WithMessage("Status must be a valid TaskItemStatus value");

        RuleFor(x => x.CreateTaskDto.Priority)
            .IsInEnum().WithMessage("Priority must be a valid TaskPriority value");
    }

}

