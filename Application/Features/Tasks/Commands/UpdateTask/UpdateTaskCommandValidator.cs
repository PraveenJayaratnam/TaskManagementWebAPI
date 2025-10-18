namespace Application.Features.Tasks.Commands;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Task ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid task status");

        RuleFor(x => x.DueDate)
            .Must(BeValidDate).WithMessage("Due date must be a valid date in yyyy-MM-dd format")
            .When(x => !string.IsNullOrEmpty(x.DueDate));
    }

    private static bool BeValidDate(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString))
            return true;

        return DateTime.TryParse(dateString, out _);
    }
}

