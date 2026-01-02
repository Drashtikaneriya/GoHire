using FluentValidation;
using RecruitmentsystemAPI.DTOs.Company;

namespace RecruitmentsystemAPI.Validators.Company
{
    public class CompanyUpdateValidator
        : AbstractValidator<CompanyUpdateDTO>
    {
        public CompanyUpdateValidator()
        {
            RuleFor(x => x.CompanyId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .WithMessage("Valid CompanyId is required.");

            RuleFor(x => x.CompanyName).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Industry).Cascade(CascadeMode.Stop)
                .MaximumLength(150);

            RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
                .EmailAddress()
                .MaximumLength(150)
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Phone).Cascade(CascadeMode.Stop)
                .MaximumLength(20);

            RuleFor(x => x.Website).Cascade(CascadeMode.Stop)
                .MaximumLength(200);

            RuleFor(x => x.Address).Cascade(CascadeMode.Stop)
                .MaximumLength(300);
        }
    }
}
