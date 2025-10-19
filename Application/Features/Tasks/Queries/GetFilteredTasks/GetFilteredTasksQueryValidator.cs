namespace Application.Features.Tasks.Queries;

public class GetFilteredTasksQueryValidator : AbstractValidator<GetFilteredTasksQuery>
{
    public GetFilteredTasksQueryValidator()
    {
        RuleFor(x => x.Filter.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.Filter.PageIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Page index must be 0 or greater");

        RuleFor(x => x.Filter.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100");

        RuleFor(x => x.Filter.SortDirection)
            .Must(BeValidSortDirection).WithMessage("Sort direction must be 'asc' or 'desc'")
            .When(x => !string.IsNullOrEmpty(x.Filter.SortDirection));

        RuleFor(x => x.Filter.SortBy)
            .Must(BeValidSortBy).WithMessage("Sort by must be one of: title, status, priority, createdat, duedate")
            .When(x => !string.IsNullOrEmpty(x.Filter.SortBy));

        RuleFor(x => x.Filter.DueDateFrom)
            .LessThanOrEqualTo(x => x.Filter.DueDateTo)
            .WithMessage("Due date from must be less than or equal to due date to")
            .When(x => x.Filter.DueDateFrom.HasValue && x.Filter.DueDateTo.HasValue);
    }

    private static bool BeValidSortDirection(string? sortDirection)
    {
        if (string.IsNullOrEmpty(sortDirection))
            return true;

        return sortDirection.ToLower() == "asc" || sortDirection.ToLower() == "desc";
    }

    private static bool BeValidSortBy(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
            return true;

        var validSortFields = new[] { "title", "status", "priority", "createdat", "duedate" };
        return validSortFields.Contains(sortBy.ToLowerInvariant());
    }
}

