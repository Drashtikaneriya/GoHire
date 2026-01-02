using FluentValidation;
using RecruitmentsystemAPI.DTOs.Candidate;

namespace RecruitmentsystemAPI.Validators.Candidate
{
    public class CandidateUpdateValidator
        : AbstractValidator<CandidateUpdateDTO>
    {
        public CandidateUpdateValidator()
        {
            RuleFor(x => x.CandidateId).Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .WithMessage("Valid CandidateId is required.");

            RuleFor(x => x.FullName).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(150);
                
            RuleFor(x => x.Phone).Cascade(CascadeMode.Stop)
                .MaximumLength(20);

            RuleFor(x => x.ResumePath).Cascade(CascadeMode.Stop)
                .MaximumLength(300);
        }
    }
}
