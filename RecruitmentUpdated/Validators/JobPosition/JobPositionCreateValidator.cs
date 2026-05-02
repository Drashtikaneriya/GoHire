using FluentValidation;
using RecruitmentsystemAPI.DTO.Jobposition;

namespace RecruitmentsystemAPI.Validators.JobPosition
{
    public class JobPositionCreateValidator : AbstractValidator<JobPositionCreateDTO>
    {
        public JobPositionCreateValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("A valid CompanyId is required.");

            RuleFor(x => x.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(150).WithMessage("Title cannot exceed 150 characters.");

            RuleFor(x => x.Location)
                .MaximumLength(150).WithMessage("Location cannot exceed 150 characters.")
                .When(x => x.Location != null);

            RuleFor(x => x.EmploymentType)
                .MaximumLength(50).WithMessage("EmploymentType cannot exceed 50 characters.")
                .When(x => x.EmploymentType != null);

            RuleFor(x => x.SalaryMin)
                .GreaterThanOrEqualTo(0).WithMessage("SalaryMin must be 0 or greater.")
                .When(x => x.SalaryMin.HasValue);

            RuleFor(x => x.SalaryMax)
                .GreaterThanOrEqualTo(0).WithMessage("SalaryMax must be 0 or greater.")
                .GreaterThanOrEqualTo(x => x.SalaryMin ?? 0).WithMessage("SalaryMax must be >= SalaryMin.")
                .When(x => x.SalaryMax.HasValue);

            RuleFor(x => x.CreatedByUserId)
                .GreaterThan(0).WithMessage("A valid CreatedByUserId is required.");
        }
    }
}
