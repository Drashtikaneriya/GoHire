using FluentValidation;
using RecruitmentsystemAPI.DTO.Application;

namespace RecruitmentsystemAPI.Validators.Application
{
    public class ApplicationCreateValidator : AbstractValidator<ApplicationCreateDTO>
    {
        public ApplicationCreateValidator()
        {
            RuleFor(x => x.JobId)
                .GreaterThan(0).WithMessage("A valid JobId is required.");

            //RuleFor(x => x.CandidateId)
            //    .GreaterThan(0).WithMessage("A valid CandidateId is required.");
        }
    }
}
