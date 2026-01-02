using FluentValidation;
using RecruitmentsystemAPI.DTOs.Interview;

namespace RecruitmentsystemAPI.Validators.Interview
{
    public class InterviewCreateValidator
        : AbstractValidator<InterviewCreateDTO>
    {
        public InterviewCreateValidator()
        {
            RuleFor(x => x.ApplicationId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .WithMessage("Valid ApplicationId is required.");

            RuleFor(x => x.InterviewerId).Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .WithMessage("Valid InterviewerId is required.");

            RuleFor(x => x.InterviewDate).Cascade(CascadeMode.Stop)
                .GreaterThan(DateTime.Now.AddMinutes(-1))
                .WithMessage("Interview date must be in the future.");

            RuleFor(x => x.Mode).Cascade(CascadeMode.Stop)
                .Must(m => m == null || m == "Online" || m == "Offline")
                .WithMessage("Mode must be Online or Offline.");

            RuleFor(x => x.Feedback).Cascade(CascadeMode.Stop)
                .MaximumLength(500);
        }
    }
}
