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
            .Must(BeValidDate).WithMessage("Due date must be a valid date in yyyy-MM-dd format")
            .When(x => !string.IsNullOrEmpty(x.CreateTaskDto.DueDate));
    }

    private static bool BeValidDate(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString))
            return true;

        return DateTime.TryParse(dateString, out _);
    }
}

