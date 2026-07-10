using BranchService.Application.Requests;
using FluentValidation;

namespace BranchService.Application.Validators.CompanyServiceValidators;

public class UpdateCompanyServiceValidator: AbstractValidator<UpdateCompanyServiceRequest>
{
    public UpdateCompanyServiceValidator()
    {
        RuleFor(s => s.ServiceDuration)
            .NotEmpty().WithMessage("ServiceDuration is required.")
            .GreaterThanOrEqualTo(15).WithMessage("Duration time ,must be greater than or equal to 15 .");
        
        RuleFor(s => s.ServiceName)
            .NotEmpty().WithMessage("Service name is required.")
            .MaximumLength(30).WithMessage("Service name cannot exceed 30 characters.");

        RuleFor(s => s.ServiceDescription)
            .NotEmpty().WithMessage("ServiceDescription is required.")
            .MaximumLength(50).WithMessage("Service description cannot exceed 50 characters.");
    }
}