using FluentValidation;
using RecruitmentsystemAPI.DTO.Role;

namespace RecruitmentsystemAPI.Validators
{
    public class RoleUpdateValidator : AbstractValidator<RoleUpdateDTO>
    {
        public RoleUpdateValidator()
        {
            RuleFor(role => role.RoleId).Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .WithMessage("Valid RoleId is required for update.");

            RuleFor(role => role.RoleName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(100).WithMessage("Role name cannot exceed 100 characters.");
        }
    }
}
