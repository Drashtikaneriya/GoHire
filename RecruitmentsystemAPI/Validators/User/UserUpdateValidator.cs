using FluentValidation;
using RecruitmentsystemAPI.DTOs;

namespace RecruitmentsystemAPI.Validators
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateDTO>
    {
        public UserUpdateValidator()
        {
            RuleFor(user => user.UserId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .WithMessage("Valid UserId is required for update.");

            RuleFor(user => user.UserName).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(user => user.Email).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(150);
                
            RuleFor(user => user.RoleId).Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .WithMessage("Valid RoleId is required.");
        }
    }
}
