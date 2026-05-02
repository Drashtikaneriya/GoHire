using FluentValidation;
using RecruitmentsystemAPI.DTO.Interview;

namespace RecruitmentsystemAPI.Validators.Interview
{
    public class InterviewCreateValidator : AbstractValidator<InterviewCreateDTO>
    {
        public InterviewCreateValidator()
        {
            RuleFor(x => x.ApplicationId)
                .GreaterThan(0).WithMessage("A valid ApplicationId is required.");

            RuleFor(x => x.RoundId)
                .GreaterThan(0).WithMessage("A valid RoundId is required.");

            RuleFor(x => x.InterviewerUserId)
                .GreaterThan(0).WithMessage("A valid InterviewerUserId is required.");

            RuleFor(x => x.InterviewDate)
                .NotEmpty().WithMessage("InterviewDate is required.")
                .GreaterThan(DateTime.Now).WithMessage("InterviewDate must be in the future.");

            RuleFor(x => x.InterviewEnd)
                .GreaterThan(x => x.InterviewDate).WithMessage("InterviewEnd must be after InterviewDate.")
                .When(x => x.InterviewEnd.HasValue);

            RuleFor(x => x.Mode)
                .MaximumLength(50).WithMessage("Mode cannot exceed 50 characters.")
                .When(x => x.Mode != null);
        }
    }
}
