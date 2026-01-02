using FluentValidation;
using RecruitmentsystemAPI.DTOs.JobPosition;

namespace RecruitmentsystemAPI.Validators.JobPosition
{
    public class JobPositionUpdateValidator
        : AbstractValidator<JobPositionUpdateDTO>
    {
        public JobPositionUpdateValidator()
        {
            RuleFor(x => x.JobId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .WithMessage("Valid JobId is required.");

            RuleFor(x => x.Title).Cascade(CascadeMode.Stop)
                .NotEmpty()
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
        }
    }
}
