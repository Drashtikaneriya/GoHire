using FluentValidation;
using RecruitmentsystemAPI.DTOs.Application;

namespace RecruitmentsystemAPI.Validators.Application
{
    public class ApplicationCreateValidator
        : AbstractValidator<ApplicationCreateDTO>
    {
        public ApplicationCreateValidator()
        {
            // JobId validation
            RuleFor(x => x.JobId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("JobId is required")
                .GreaterThan(0).WithMessage("JobId must be greater than 0");

            // CandidateId validation
            RuleFor(x => x.CandidateId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("CandidateId is required")
                .GreaterThan(0).WithMessage("CandidateId must be greater than 0");

            // Status validation (Create time)
            RuleFor(x => x.Status)
                .Cascade(CascadeMode.Stop)
                .Must(status => status == null || status == "Applied")
                .WithMessage("Status must be 'Applied' at creation time");

            // HRNotes validation
            RuleFor(x => x.HRNotes)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(500)
                .WithMessage("HR Notes cannot exceed 500 characters");
        }
    }
}
