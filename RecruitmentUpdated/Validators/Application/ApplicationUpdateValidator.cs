using FluentValidation;
using RecruitmentsystemAPI.DTO.Application;

namespace RecruitmentsystemAPI.Validators.Application
{
    public class ApplicationUpdateValidator : AbstractValidator<ApplicationUpdateDTO>
    {
        private static readonly string[] AllowedStatuses = { "Applied", "Interviewing", "Offered", "Rejected", "Withdrawn" };

        public ApplicationUpdateValidator()
        {
            RuleFor(x => x.Status)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Status is required.")
                .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.")
                .Must(s => AllowedStatuses.Contains(s)).WithMessage($"Status must be one of: {string.Join(", ", AllowedStatuses)}.");
        }
    }
}
