using FluentValidation;
using RecruitmentsystemAPI.DTOs.JobPosition;

namespace RecruitmentsystemAPI.Validators.JobPosition
{
    public class JobPositionCreateValidator
        : AbstractValidator<JobPositionCreateDTO>
    {
        public JobPositionCreateValidator()
        {
            RuleFor(x => x.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Job title is required.")
                .MaximumLength(200);

            RuleFor(x => x.Location).Cascade(CascadeMode.Stop)
                .MaximumLength(150);

            RuleFor(x => x.Type).Cascade(CascadeMode.Stop)
                .Must(t =>
                    t == null ||
                    t == "Full-time" ||
                    t == "Part-time" ||
                    t == "Remote")
                .WithMessage("Type must be Full-time, Part-time, or Remote.");

            RuleFor(x => x.SalaryRange).Cascade(CascadeMode.Stop)
                .MaximumLength(100);

            RuleFor(x => x.CreatedBy).Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .WithMessage("Valid CreatedBy (UserId) is required.");
        }
    }
}
