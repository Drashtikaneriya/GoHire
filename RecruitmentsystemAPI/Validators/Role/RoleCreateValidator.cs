using FluentValidation;
using RecruitmentsystemAPI.DTO.Role;

namespace RecruitmentsystemAPI.Validators
{
    public class RoleCreateValidator : AbstractValidator<RoleCreateDTO>
    {
        public RoleCreateValidator()
        {
            RuleFor(role => role.RoleName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(100).WithMessage("Role name cannot exceed 100 characters.");
        }
    }
}
