using FluentValidation;
using RecruitmentsystemAPI.DTO.Company;

namespace RecruitmentsystemAPI.Validators.Company
{
    public class CompanyUpdateValidator : AbstractValidator<CompanyUpdateDTO>
    {
        public CompanyUpdateValidator()
        {
            RuleFor(x => x.CompanyName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("CompanyName is required.")
                .MaximumLength(150).WithMessage("CompanyName cannot exceed 150 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("A valid email is required.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters.")
                .When(x => x.Phone != null);

            RuleFor(x => x.Website)
                .MaximumLength(200).WithMessage("Website cannot exceed 200 characters.")
                .When(x => x.Website != null);

            RuleFor(x => x.Address)
                .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.")
                .When(x => x.Address != null);
        }
    }
}
