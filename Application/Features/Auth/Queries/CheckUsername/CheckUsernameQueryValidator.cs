namespace Application.Features.Auth.Queries
{
    public class CheckUsernameQueryValidator : AbstractValidator<CheckUsernameQuery>
    {
        public CheckUsernameQueryValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters");
        }
    }
}

