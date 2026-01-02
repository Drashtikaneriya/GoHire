using FluentValidation;
using RecruitmentsystemAPI.DTOs.Candidate;

namespace RecruitmentsystemAPI.Validators.Candidate
{
    public class CandidateCreateValidator
        : AbstractValidator<CandidateCreateDTO>
    {
        public CandidateCreateValidator()
        {
            RuleFor(x => x.FullName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(150);

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(150);

            RuleFor(x => x.Phone)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(20);

            RuleFor(x => x.ResumePath)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(300);
        }
    }
}
