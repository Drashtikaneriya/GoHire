using FluentValidation;
using RecruitmentsystemAPI.DTO.Candidate;

namespace RecruitmentsystemAPI.Validators.Candidate
{
    public class CandidateCreateValidator : AbstractValidator<CandidateCreateDTO>
    {
        public CandidateCreateValidator()
        {
            //RuleFor(x => x.UserId)
            //    .GreaterThan(0).WithMessage("A valid UserId is required.");

            RuleFor(x => x.LinkedInUrl)
                .MaximumLength(200).WithMessage("LinkedInUrl cannot exceed 200 characters.")
                .When(x => x.LinkedInUrl != null);

            RuleFor(x => x.PortfolioUrl)
                .MaximumLength(200).WithMessage("PortfolioUrl cannot exceed 200 characters.")
                .When(x => x.PortfolioUrl != null);
        }
    }
}
