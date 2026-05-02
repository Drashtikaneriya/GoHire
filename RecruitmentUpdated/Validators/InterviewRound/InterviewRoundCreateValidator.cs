using FluentValidation;
using RecruitmentsystemAPI.DTO.InterviewRound;

namespace RecruitmentsystemAPI.Validators.InterviewRound
{
    public class InterviewRoundCreateValidator : AbstractValidator<InterviewRoundCreateDTO>
    {
        public InterviewRoundCreateValidator()
        {
            RuleFor(x => x.RoundName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("RoundName is required.")
                .MaximumLength(100).WithMessage("RoundName cannot exceed 100 characters.");
        }
    }
}
