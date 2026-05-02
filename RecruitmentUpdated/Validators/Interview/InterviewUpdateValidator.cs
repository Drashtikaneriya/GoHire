using FluentValidation;
using RecruitmentsystemAPI.DTO.Interview;

namespace RecruitmentsystemAPI.Validators.Interview
{
    public class InterviewUpdateValidator : AbstractValidator<InterviewUpdateDTO>
    {
        private static readonly string[] AllowedResults = { "Pass", "Fail", "Pending" };

        public InterviewUpdateValidator()
        {
            RuleFor(x => x.InterviewId)
                .GreaterThan(0).WithMessage("A valid InterviewId is required.");

            RuleFor(x => x.InterviewEnd)
                .GreaterThan(x => x.InterviewDate ?? DateTime.MinValue).WithMessage("InterviewEnd must be after InterviewDate.")
                .When(x => x.InterviewEnd.HasValue && x.InterviewDate.HasValue);

            RuleFor(x => x.Mode)
                .MaximumLength(50).WithMessage("Mode cannot exceed 50 characters.")
                .When(x => x.Mode != null);

            RuleFor(x => x.Result)
                .MaximumLength(50).WithMessage("Result cannot exceed 50 characters.")
                .Must(r => AllowedResults.Contains(r)).WithMessage($"Result must be one of: {string.Join(", ", AllowedResults)}.")
                .When(x => !string.IsNullOrEmpty(x.Result));
        }
    }
}
