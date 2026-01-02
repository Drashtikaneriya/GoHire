using FluentValidation;
using RecruitmentsystemAPI.DTOs.Interview;

namespace RecruitmentsystemAPI.Validators.Interview
{
    public class InterviewUpdateValidator
        : AbstractValidator<InterviewUpdateDTO>
    {
        public InterviewUpdateValidator()
        {
            RuleFor(x => x.InterviewId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .WithMessage("Valid InterviewId is required.");

            RuleFor(x => x.Mode).Cascade(CascadeMode.Stop)
                .Must(m => m == null || m == "Online" || m == "Offline")
                .WithMessage("Mode must be Online or Offline.");

            RuleFor(x => x.Result).Cascade(CascadeMode.Stop)
                .Must(r =>
                    r == null ||
                    r == "Pending" ||
                    r == "Selected" ||
                    r == "Rejected")
                .WithMessage("Result must be Pending, Selected, or Rejected.");

            RuleFor(x => x.Feedback).Cascade(CascadeMode.Stop)
                .MaximumLength(500);
        }
    }
}
