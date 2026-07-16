using BranchService.Application.Requests;
using BranchService.Application.Validators.CompanyServiceValidators;
using FluentValidation.TestHelper;

namespace BranchService.UnitTest.BranchService.Application.Tests.ValidatorTests.CompanyServiceValidatorTests;

public class UpdateCompanyServiceValidatorTests
{
    private readonly UpdateCompanyServiceValidator _validator;

    public UpdateCompanyServiceValidatorTests()
    {
        _validator = new UpdateCompanyServiceValidator();
    }

    [Fact]
    public async Task Validator_WhenCommandIsValid_ShouldNotHaveAnyValidationErrors()
    {
        var command = new UpdateCompanyServiceRequest
        {
            ServiceName = "Test Name",
            ServiceDescription = "Test Service Description",
            ServiceDuration = 45,
        };

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }


    [Fact]
    public async Task Validator_WhenServiceNameIsEmpty_ShouldHaveValidationError()
    {
        var command = new UpdateCompanyServiceRequest
        {
            ServiceName = "",
            ServiceDescription = "Test Service Description"
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ServiceName)
            .WithErrorMessage("Service name is required.");
    }

    [Fact]
    public async Task Validator_WhenServiceDescriptionIsEmpty_ShouldHaveValidationError()
    {
        var command = new UpdateCompanyServiceRequest
        {
            ServiceName = "Test Name",
            ServiceDescription = ""
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.ServiceDescription)
            .WithErrorMessage("ServiceDescription is required.");
    }


    [Fact]
    public async Task Validator_WhenServiceDurationIsSmallerThan15_ShouldHaveValidationError()
    {
        var command = new UpdateCompanyServiceRequest
        {
            ServiceName = "Test Name",
            ServiceDescription = "Test Service Description",
            ServiceDuration = 10
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.ServiceDuration)
            .WithErrorMessage("Duration time ,must be greater than or equal to 15 .");
    }
}