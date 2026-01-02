using FluentValidation;
using RecruitmentsystemAPI.DTOs.Application;

namespace RecruitmentsystemAPI.Validators.Application
{
    public class ApplicationUpdateValidator : AbstractValidator<ApplicationUpdateDTO>
    {
        public ApplicationUpdateValidator()
        {
            RuleFor(x => x.ApplicationId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0);

                RuleFor(x => x.Status).Cascade(CascadeMode.Stop)
                .Must(s =>
                    s == "Shortlisted" ||
                    s == "Rejected")
                .WithMessage("Status must be Shortlisted or Rejected");

            RuleFor(x => x.HRNotes).Cascade(CascadeMode.Stop)
                .MaximumLength(500);
        }
    }

}
