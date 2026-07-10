using BranchService.Application.UseCases.CompanyServices.Commands.CreateService;
using BranchService.Application.Validators.CompanyServiceValidators;
using FluentValidation.TestHelper;

namespace BranchService.UnitTest.BranchService.Application.Tests.ValidatorTests.CompanyServiceValidatorTests;

public class CreateCompanyServiceValidatorTests
{
    private readonly CreateCompanyServiceValidator _validator;

    public CreateCompanyServiceValidatorTests()
    {
        _validator = new CreateCompanyServiceValidator();
    }

    [Fact]
    public async Task Validator_WhenCommandIsValid_ShouldNotHaveAnyValidationErrors()
    {
        var command = new CreateServiceCommand(
            1,
            1,
            "Test Name",
            "Test Service Description",
            45);

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }


    [Fact]
    public async Task Validator_WhenServiceNameIsEmpty_ShouldHaveValidationError()
    {
        var command = new CreateServiceCommand(
            1,
            1,
            "",
            "Test Service Description",
            45);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ServiceName)
            .WithErrorMessage("Service name is required.");
    }

    [Fact]
    public async Task Validator_WhenServiceDescriptionIsEmpty_ShouldHaveValidationError()
    {
        var command = new CreateServiceCommand(
            1,
            1,
            "Test Name",
            "",
            45);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.ServiceDescription)
            .WithErrorMessage("ServiceDescription is required.");
    }
    
    [Theory]
    [InlineData(0)]
    public void Validator_WhenCompanyIdIsInvalid_ShouldHaveValidationError(int invalidCompanyId)
    {
        var command = new CreateServiceCommand(
            invalidCompanyId,
            1,
            "Test Name",
            "",
            45);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.CompanyId)
            .WithErrorMessage("CompanyId is required.");
    }
    
    [Theory]
    [InlineData(0)]
    public void Validator_WhenBranchIdIsInvalid_ShouldHaveValidationError(int invalidBranchId)
    {
        var command = new CreateServiceCommand(
            1,
            invalidBranchId,
            "Test Name",
            "",
            45);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.BranchId)
            .WithErrorMessage("BranchId is required.");
    }
    
    [Fact]
    public async Task Validator_WhenServiceDurationIsSmallerThan15_ShouldHaveValidationError()
    {
        var command = new CreateServiceCommand(
            1,
            1,
            "Test Name",
            "",
            10);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.ServiceDuration)
            .WithErrorMessage("Duration time ,must be greater than or equal to 15 .");
    }
}