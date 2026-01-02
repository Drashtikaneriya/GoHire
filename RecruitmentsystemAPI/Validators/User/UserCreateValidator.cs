using FluentValidation;
using RecruitmentsystemAPI.DTOs;

namespace RecruitmentsystemAPI.Validators
{
    public class UserCreateValidator : AbstractValidator<UserCreateDTO>
    {
        public UserCreateValidator()
        {
            RuleFor(user => user.UserName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("User name is required.")
                .MaximumLength(150).WithMessage("User name cannot exceed 150 characters.");

            RuleFor(user => user.Email).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.");

            RuleFor(user => user.Password).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(300).WithMessage("Password cannot exceed 300 characters.");

            RuleFor(user => user.RoleId).Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Valid RoleId is required.");
        }
    }
}
