using FluentValidation;
using RecruitmentsystemAPI.DTO.User;

namespace RecruitmentsystemAPI.Validators.User
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateDTO>
    {
        public UserUpdateValidator()
        {
            RuleFor(x => x.FullName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("FullName is required.")
                .MaximumLength(100).WithMessage("FullName cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(x => x.Phone)
                .MaximumLength(15).WithMessage("Phone cannot exceed 15 characters.")
                .When(x => x.Phone != null);

            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("A valid RoleId is required.");
        }
    }
}
