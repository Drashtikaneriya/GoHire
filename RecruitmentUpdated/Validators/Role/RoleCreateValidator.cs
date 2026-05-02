using FluentValidation;
using RecruitmentsystemAPI.DTO.Role;

namespace RecruitmentsystemAPI.Validators.Role
{
    public class RoleCreateValidator : AbstractValidator<RoleCreateDTO>
    {
        public RoleCreateValidator()
        {
            RuleFor(x => x.RoleName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("RoleName is required.")
                .MaximumLength(50).WithMessage("RoleName cannot exceed 50 characters.");
        }
    }
}
