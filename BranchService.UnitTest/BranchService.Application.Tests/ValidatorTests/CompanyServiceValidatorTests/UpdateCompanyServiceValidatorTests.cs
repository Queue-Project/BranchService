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
            CompanyId = 1,
            ServiceName = "Test Name",
            ServiceDescription = "Test Service Description"
        };

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }


    [Fact]
    public async Task Validator_WhenServiceNameIsEmpty_ShouldHaveValidationError()
    {
        var command = new UpdateCompanyServiceRequest
        {
            CompanyId = 1,
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
            CompanyId = 1,
            ServiceName = "Test Name",
            ServiceDescription = ""
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.ServiceDescription)
            .WithErrorMessage("ServiceDescription is required.");
    }

    [Theory]
    [InlineData(0)]
    public void Validator_WhenCompanyIdIsInvalid_ShouldHaveValidationError(int invalidCompanyId)
    {
        var command = new UpdateCompanyServiceRequest
        {
            CompanyId = invalidCompanyId,
            ServiceName = "Test Name",
            ServiceDescription = "Test Service Description"
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.CompanyId)
            .WithErrorMessage("CompanyId is required.");
    }
}